﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LibraryService.DataContracts
{
    [DataContract]
    public class AuthorInfo
    {
        [DataMember]
        public string firstName;

        [DataMember]
        public string lastName;
    }

}
