using ACQ.CoreAPI.Utility.Email;
using DDPAdmin.Services.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MOD.Models
{
    public class EncryptPassword
    {
        public async Task<int> EncryptPasswordAllUser()
        {
            int Res = 0;
            using (var _context = new MODEntities())
            {
                var isValid = _context.tbl_tbl_User.ToList();
                foreach (var i in isValid)
                {
                    //if (i.Pswd_Salt == null)
                    {
                        string GenPwd = Cipher.Encrypt_Portal(i.Password);
                       // string GenPwd = i.Password;
                        //string GetSalt = GeneratedPassword.CreateSalt(10);
                       // string hashString = GeneratedPassword.GenarateHash(GenPwd, GetSalt);
                        i.Pswd_Salt = GenPwd;
                        i.Flag = "Y";
                        _context.SaveChanges();
                    }
                }
                return Res;
            }
        }
    }
}