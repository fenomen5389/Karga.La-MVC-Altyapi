using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Constants
{
    public static class Messages
    {
        public static string Ok = "Sistem iş katmanı çerçevesince işlem onaylandı.";
        public static string ListEmpty = "Sistem boş küme döndürdü.";
        public static string Null = "Boş veri döndü.";
        public static string WrongCredentials = "Giriş bilgileri hatalı.";
        public static string CategoryNameCannotBeEmpty = "Kategori adı boş olamaz.";
        public static string CategoryParentCannotBeItself = "Bir kategori kendisini ebeveyn sınıf olarak kabul edemez.";
        public static string PermissionDenied = "Bu işlemi gerçekleştirebilecek yetkiniz yok.";
    }
}
