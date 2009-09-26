using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace UtnEmall.Server.Core
{
    /// <summary>
    /// Atributo para permitir a los servicios WCF preservar referencias circulares.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    sealed public class ReferencePreservingDataContractFormatAttribute : Attribute, IOperationBehavior
    {
        #region Instance Methods

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {
            IOperationBehavior innerBehavior = new ReferencePreservingDataContractSerializerOperationBehavior(operationDescription);
            innerBehavior.ApplyClientBehavior(operationDescription, clientOperation);
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            IOperationBehavior innerBehavior = new ReferencePreservingDataContractSerializerOperationBehavior(operationDescription);
            innerBehavior.ApplyDispatchBehavior(operationDescription, dispatchOperation);
        }

        public void Validate(OperationDescription operationDescription)
        {
        }

        #endregion
    }

    /// <summary>
    /// DataContractSerializarOperationBehavior que preserva referencias circulares en los objetos.
    /// </summary>
    class ReferencePreservingDataContractSerializerOperationBehavior : DataContractSerializerOperationBehavior
    {
        #region Constructors

        public ReferencePreservingDataContractSerializerOperationBehavior(OperationDescription operationDescription) : base(operationDescription) { }

        #endregion

        #region Static Methods

        private static XmlObjectSerializer CreateDataContractSerializer(Type type, string name, string ns, IList<Type> knownTypes)
        {
            return CreateDataContractSerializer(type, name, ns, knownTypes);
        }

        #endregion

        #region Instance Methods

        public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns, IList<Type> knownTypes)
        {
            return CreateDataContractSerializer(type, name, ns, knownTypes);
        }

        public override XmlObjectSerializer CreateSerializer(Type type, XmlDictionaryString name, XmlDictionaryString ns, IList<Type> knownTypes)
        {
            return new DataContractSerializer(type, name, ns, knownTypes,
                0x7FFF /*maxItemsInObjectGraph*/,
                false/*ignoreExtensionDataObject*/,
                true/*preserveObjectReferences*/,
                null/*dataContractSurrogate*/);
        }

        #endregion
    }
}
