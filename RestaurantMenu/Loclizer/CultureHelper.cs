using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestaurantMenu.Loclizer
{
    public class CultureHelper : Controller
    {
        public static string EnglishCultureName { get { return "en"; } }
        public static string ArabicCultureName { get { return "ar"; } }
        public static string CultureName
        {
            get
            {

                return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            }
        }
        public static bool IsArabic { get { return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "ar"; } }
        public static bool IsEnglish { get { return Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "en"; } }
        public void InitializeCulture(string cluture, string returnUrl)
        {
            Response.Cookies.Append(
                  CookieRequestCultureProvider.DefaultCookieName,
                  CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cluture)),
                  new CookieOptions { Expires = DateTimeOffset.UtcNow.AddHours(1) }

                  );
            var cultureInfo = new CultureInfo(cluture)
            {
                DateTimeFormat =
                {
                    Calendar = new GregorianCalendar(),
                    ShortDatePattern = "dd/MM/yyyy",
                    LongDatePattern = "dd/MM/yyyy",
                    FullDateTimePattern = "dd/MM/yyyy hh:mm:ss"
                }
            };
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            // return LocalRedirect(returnUrl);

        }
        public static bool IsStringEnglish(string text)
        {
            if (string.IsNullOrEmpty(text)) return true;
            text = text.Trim();
            const string enOnlyPattern = @"^[a-zA-Z0-9]*$";
            if (text.Length > 3)
                return Regex.IsMatch(RemoveSpecialChars(text).Substring(0, 3), enOnlyPattern);
            else
                return Regex.IsMatch(RemoveSpecialChars(text), enOnlyPattern);
        }

        private static string RemoveSpecialChars(string text)
        {
            var r = new Regex(@"\s|\-|'", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(text, String.Empty);
        }
    }

}
