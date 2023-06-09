﻿namespace Entities.Entities
{
    public class UserItem
    {
        public int Id { get; set; }
        public int IdRol { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<ProductItem> Product { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public string EncryptedPassword { get; set; }
        public string EncryptedToken { get; set; }
        public DateTime TokenExpireDate { get; set; }
    }
}