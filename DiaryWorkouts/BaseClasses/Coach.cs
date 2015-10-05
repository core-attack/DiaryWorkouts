using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace DiaryWorkouts.BaseClasses
{
    /// <summary>
    /// Тренер
    /// </summary>
    class Coach
    {
        #region [Скрытые поля]

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        long _id;

        #endregion

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public long id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Конструктор класса Athlete
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Фамилия</param>
        /// <param name="lastName">Имя</param>
        /// <param name="fatherName">Отчество</param>
        /// <param name="password">Пароль</param>
        /// <param name="phone">Контактный телефон</param>
        /// <param name="sex">Пол</param>
        /// <param name="createDate">Дата создания записи спортсмена в БД</param>
        /// <param name="sportTypeId">Идентификатор вида спорта</param>
        /// <param name="sportCategory">Идентификатор спортивного разряда</param>
        /// <param name="address">Адрес</param>
        /// <param name="place">Место учебы/работы</param>
        public Coach(long id, byte sportTypeId, byte sportCategory, string address, string place)
        {
        }

        public Coach(DbDataReader reader)
        { }

        /// <summary>
        /// Возвращает строку sql-скрипта для вставки данной записи в БД
        /// </summary>
        /// <returns></returns>
        public string GetSQLInsert()
        {
            return "";
        }
    }
}
