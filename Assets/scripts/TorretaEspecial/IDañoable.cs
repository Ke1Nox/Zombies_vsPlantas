using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.scripts.TorretaEspecial
{
    public interface IDañoable
    {
        int Vida { get; }
        Transform transform { get; }
    }

}
