using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_SQLtest
{
    public class Produit
    {
        #region Variables
        string id;
        string nom;
        string num_serie;
        #endregion

        #region get and set
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public string Nom
        {
            get
            {
                return nom;
            }
            set
            {
                nom = value;
            }
        }
        public string Num_serie
        {
            get
            {
                return num_serie;
            }
            set
            {
                num_serie = value;
            }
        }
        #endregion

        public Produit() {} 
    }
}
