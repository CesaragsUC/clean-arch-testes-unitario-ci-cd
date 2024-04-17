using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.Test
{
    [CollectionDefinition(nameof(DocumentoCollection))]
    public class DocumentoCollection : ICollectionFixture<DocumentoFixture>
    {
    }

    public class DocumentoFixture 
    {

        public  string CriarCPFValido()
        {
            return "529.982.247-25";
        }

        public string CriarCPFInvalido()
        {
            return "111.111.111-11";
        }

        public string CriarRGValido()
        {
            return "11.111.111-1";
        }

        public string CriarRGInvalido()
        {
            return "1";
        }
    }
}
