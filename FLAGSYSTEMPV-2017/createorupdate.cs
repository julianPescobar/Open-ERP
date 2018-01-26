using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FLAGSYSTEMPV_2017
{
    class createorupdate
    {
        public static string status; //lo uso para todo lo que se pueda modificar como clientes proveedores, etc.
        //para no tener que crear otro formulario cuando uno quiere editar algun cliente esto permite abrir un form 
        //en modo creacion o modificacion entonces se ahorra tiempo y espacio en disco.
        public static string itemid; //agarro el id de lo que quiero modificar (si es que pongo status update en vez de create)

    }
}
