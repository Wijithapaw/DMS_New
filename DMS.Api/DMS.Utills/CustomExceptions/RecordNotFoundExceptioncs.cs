using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Utills.CustomExceptions
{
    public class RecordNotFoundException : DMSException
    {
        public RecordNotFoundException() : base ("Record not found") { }

        public RecordNotFoundException(string entity) : base($"{entity} not found") { }

        public RecordNotFoundException(string entity, int id) : base($"{entity} (Id: {id}) not found" ) { }
    }
}
