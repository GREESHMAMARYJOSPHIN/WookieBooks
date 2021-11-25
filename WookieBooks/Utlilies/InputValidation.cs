using WookieBooks.Models;

namespace WookieBooks.Utlilies
{
    public static class InputValidation
    {
        public static bool validateRequest(Book book)
        {
            bool result = true;
            if(book.Price <= 0)
            {
                result = false;
            }

            return result;
        }
    }
}
