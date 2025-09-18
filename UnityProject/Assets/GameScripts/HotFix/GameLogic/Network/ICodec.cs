using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public interface ICodec
    {
        byte[] Encode(object message);
        object Decode(byte[] data);
    }
}
