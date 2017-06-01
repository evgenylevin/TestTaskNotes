using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProfiGroup.DomainModel
{
    public class Note
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DateChanged { get; set; }

        public bool IsNew
        {
            get
            {
                return Id == Guid.Empty;
            }
        }

        public bool IsDeleted
        {
            get
            {
                return string.IsNullOrEmpty(Title) && string.IsNullOrEmpty(Content);
            }
        }
    }
}
