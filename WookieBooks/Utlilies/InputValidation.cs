using WookieBooks.Models;

namespace WookieBooks.Utlilies
{
    public static class InputValidation
    {
        /// <summary>
        /// Validate the book object in the request
        /// </summary>
        /// <param name="book"></param>
        /// <returns value="true/false"></returns>
        public static bool validateRequest(Book book)
        {
            bool result = true;
            //Request considered valid if Price in the request is greater than 0.00
            if(book.Price <= 0) 
            {
                result = false;
            }

            return result;
        }
    }
}
