using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEbook.Model
{
    class AutoComplete
    {
        public class CompleteTitle
        {
            public string[] keywords { get; set; }
            public bool ok { get; set; }
        }

    }
}


/**
 * {
 *      "keywords":
 *      [
 *          "武动乾坤",
 *          "武动乾坤续集之大千世界",
 *          "武动乾坤番外之冰灵族",
 *          "武动乾坤续集",
 *          "武动时空",
 *          "武动韩娱",
 *          "武动乾坤冰灵族",
 *          "武动乾坤后续",
 *          "武动龙珠",
 *          "武动苍冥"
 *      ],
 *      "ok":true
 *  }
 */
