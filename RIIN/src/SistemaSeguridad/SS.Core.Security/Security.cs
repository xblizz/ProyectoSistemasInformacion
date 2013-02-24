using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Xml.Linq;

using SS.Core.Entities;
using SS.Core.Security.Authorization;
using SS.Core.Security.Menu;

namespace SS.Core.Security
{
    public static class Security
    {
        //const string key = @"ABCDEFGHIJKLMÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz";
        
        static readonly RiinContainer db = new RiinContainer();
        const string key = @"ABCDEFGHIJKLMÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz";
        public static string Encript(string password)
        {
            byte[] keyArray;
            var Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(password);
            var hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            var tdes = new TripleDESCryptoServiceProvider{Key = keyArray, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7};
            var cTransform = tdes.CreateEncryptor();
            var ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);
            tdes.Clear();
            //se regresa el resultado en forma de una cadena
            return Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
        }

        public static string Decript(string password)
        {
            byte[] keyArray;
            var Array_a_Descifrar = Convert.FromBase64String(password);
            var hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();
            var tdes = new TripleDESCryptoServiceProvider{Key = keyArray, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7};
            var cTransform = tdes.CreateDecryptor();
            var resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        private static byte[] CreateCriptoBase(byte[] arrayPass)
        {
            byte[] arrayResultado;
            TripleDESCryptoServiceProvider tdes = null;
            //const string key = @"ABCDEFGHIJKLMÑOPQRSTUVWXYZabcdefghijklmnñopqrstuvwxyz";
            try
            {
                var hash = new MD5CryptoServiceProvider();
                var keyArray = hash.ComputeHash(Encoding.UTF8.GetBytes(key));
                hash.Clear();
                tdes = new TripleDESCryptoServiceProvider { Key = keyArray, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
                var cTransform = tdes.CreateEncryptor();
                arrayResultado = cTransform.TransformFinalBlock(arrayPass, 0, arrayPass.Length);
            }
            finally
            {
                if (tdes != null) tdes.Clear();
            }

            return arrayResultado;
        }

        public static bool ExistUser(string userName)
        {
            var usr = GetUserByUserName(userName);
            return usr != null;
        }

        public static Usuario GetUserByUserName(string userName)
        {
            return db.Usuarios.FirstOrDefault(user => user.UserName.Equals(userName));
        }

        public static bool ValidateUser(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                var usr = db.Usuarios.FirstOrDefault(user => user.UserName.Equals(userName));
                if (usr != null)
                {
                    //var passDescrypt = Decript(usr.Password);//xb quitar
                    //return passDescrypt.Equals(password);
                    return true;
                }
                return false;
            }
            return false;
        }

       public static string GetPerfilByUserName(string userName)
       {
           var usr = db.Usuarios.FirstOrDefault(u => u.UserName == userName);
           if (usr != null)
           {
               var userId = usr.Id;
               var firstOrDefault = db.relPerfilesUsuarios.FirstOrDefault(p => p.UsuarioId == userId);
               if (firstOrDefault != null)
               {
                   var id = firstOrDefault.Perfil.Id;
                   var perfil = Enum.GetName(typeof(PerfilesEnum), id);
                   return perfil;
               }
           }
           return string.Empty;
       }

        public static List<MenuItem> GetMenu4ThisUser(string userName)
        {
            var listMenu = new List<MenuItem>();
            var xDoc = XDocument.Load(HttpContext.Current.Server.MapPath(@"~/MenuActionSecurity.xml"));
            IEnumerable<string> attr, attr2;
            var perfilName = GetPerfilByUserName(userName);//GetPerfilName();
            foreach (var menuElem in xDoc.Descendants("menuitem"))
            {
                var xAttribute = menuElem.Attribute("perfiles");
                attr = xAttribute.Value.Split(',').Select(x => x.Trim());
                if (attr.Contains(perfilName) && menuElem.Attribute("menu").Value.ToLower() == "off")
                {
                    var menu = new MenuItem
                    {
                        PathImg = menuElem.Attribute("pathImg") != null ? menuElem.Attribute("pathImg").Value : string.Empty,
                        Title = menuElem.Attribute("title") != null ? menuElem.Attribute("title").Value : string.Empty
                    };

                    foreach (var option in menuElem.Elements())
                    {
                        if (option.Attribute("perfiles") == null)
                        {
                            menu.Chose.Add(new Chose
                            {
                                Controller = option.Attribute("controller") != null ? option.Attribute("controller").Value : string.Empty,
                                Action = option.Attribute("action") != null ? option.Attribute("action").Value.Split(',')[0] : string.Empty,
                                Text = option.Value
                            });
                        }
                        else if (!string.IsNullOrEmpty(option.Attribute("perfiles").Value))
                        {
                            attr2 = option.Attribute("perfiles").Value.Split(',').Select(x => x.Trim());
                            if (attr2.Contains(perfilName))
                            {
                                menu.Chose.Add(new Chose
                                {
                                    Controller = option.Attribute("controller") != null ? option.Attribute("controller").Value : string.Empty,
                                    Action = option.Attribute("action") != null ? option.Attribute("action").Value.Split(',')[0] : string.Empty,
                                    Text = option.Value
                                });
                            }
                        }
                    }
                    listMenu.Add(menu);
                }
            }
            return listMenu;
        }
    }
}
