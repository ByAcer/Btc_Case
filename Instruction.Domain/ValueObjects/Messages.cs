using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruction.Domain.ValueObjects
{
    public class Messages
    {
        #region ApplicationService
        public static readonly string SUCCESS = "İşlem başarıyla gerçekleştirildi.";
        public static readonly string NO_ACTIVE_INSTRUCTURE = "Kullanıcının aktif bir emri bulunmamaktadır.";
        public static readonly string EXISTS_ACTIVE_INSTRUCTURE = "Kullanıcının zaten aktif bir emri bulunmaktadır.";
        #endregion

        #region Validation

        public static readonly string IS_BETWEEN_ACCEPTABLE_DAYS_VALIDATION = "İşlemler sadece ayın 1-28 günleri arasında gerçekleştirilmektedir!";
        public static readonly string AMOUNT_VALIDATION = "Amount alanı 100 - 20.000 arasında olmalı!";

        #endregion
    }
}
