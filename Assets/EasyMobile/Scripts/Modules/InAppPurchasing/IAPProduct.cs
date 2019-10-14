using UnityEngine;
using System.Collections;

namespace EasyMobile
{
    public enum IAPProductType
    {
        Consumable,
        NonConsumable,
        Subscription
    }

    /// <summary>
    /// Represents an in-app product in the Easy Mobile APIs. This class is
    /// different from <see cref="UnityPurchasing.Product"/> but both
    /// are programmatic representations of real world in-app products.
    /// </summary>
    [System.Serializable]
    public class IAPProduct
    {
        /// <summary>
        /// Product name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get { return _name; } }

        /// <summary>
        /// The unified Id of the product.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get { return _id; } }

        /// <summary>
        /// Product type.
        /// </summary>
        /// <value>The type.</value>
        public IAPProductType Type { get { return _type; } }

        /// <summary>
        /// Product price.
        /// </summary>
        /// <value>The price.</value>
        public string Price { get { return _price; } }

        /// <summary>
        /// Product description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get { return _description; } }

        /// <summary>
        /// Store-specific product Ids, these Ids if given will override the unified Id for the corresponding stores.
        /// </summary>
        /// <value>The store specific identifiers.</value>
        public StoreSpecificId[] StoreSpecificIds { get { return _storeSpecificIds; } }

        // Required
        [SerializeField]
        private string _name = null;
        [SerializeField]
        private IAPProductType _type = IAPProductType.Consumable;
        [SerializeField]
        private string _id = null;

        // Optional
        [SerializeField]
        private string _price = null;
        [SerializeField]
        private string _description = null;
        [SerializeField]
        private StoreSpecificId[] _storeSpecificIds = null;

#if UNITY_EDITOR
        // Editor-use via reflection only, hence the warning suppression.
#pragma warning disable 0414
        [SerializeField]
        private bool _isEditingAdvanced = false;
#pragma warning restore 0414
#endif

        [System.Serializable]
        public class StoreSpecificId
        {
            public IAPStore store;
            public string id;
        }
    }
}

