namespace EPM_Blogger.Application.DTOs.Parameters
{
    public class PostQueryParameters
    {
        private int _page = 1;
        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                // Page can't be negative
                if (value > 1) 
                {
                    _page = value;
                }
            }
        }
        private int _size = 10;
        public int Size {
            get
            {
                return _size;
            }
            set
            {
                if (_size > 10)
                _size = value;
            }
        }
    }
}
