using System.Collections.Generic;
using Salvage.ClothingCuller.Configuration;
using UnityEngine;

namespace Salvage.ClothingCuller.Components
{
    public class ClothingCuller : MonoBehaviour
    {
        private List<Occludee> registeredOccludees;

        #region SerializeFields

        [SerializeField] private ClothingCullerConfiguration config;
        [SerializeField] private Transform rigRoot;

        #endregion

        #region Unity Messages

        private void Awake()
        {
            registeredOccludees = new List<Occludee>();
        }

        private void Start()
        {
            ValidateSerializeFields();
        }

        #endregion

        #region Private Methods

        private void ValidateSerializeFields()
        {
            if (config == null)
            {
                Debug.LogError($"The {nameof(config)} field must be assigned.");
                return;
            }

            if (config.IsModularClothingWorkflowEnabled && rigRoot == null)
            {
                Debug.LogWarning($"Modular clothing workflow requires the {nameof(rigRoot)} field to be assigned.");
            }
        }

        private void ResolveBones(Occludee occludee)
        {
            #region Defense

            if (!occludee.Config.SkinnedMeshData.IsInitialized)
            {
                Debug.LogError($"Unable to resolve bones for '{occludee.name}' - config contains no {nameof(SkinnedMeshData)}.", this);
                return;
            }

            #endregion

            foreach (SkinnedMeshRenderer skinnedMeshRenderer in occludee.SkinnedMeshRenderers)
            {
                occludee.Config.SkinnedMeshData.ResolveBones(skinnedMeshRenderer, rigRoot);
            }
        }

        private void ClearBones(Occludee occludee)
        {
            foreach (SkinnedMeshRenderer skinnedMeshRenderer in occludee.SkinnedMeshRenderers)
            {
                skinnedMeshRenderer.rootBone = null;
                skinnedMeshRenderer.bones = null;
            }
        }

        #endregion

        #region Public Methods

        public void Register(Occludee occludeeToRegister)
        {
            #region Defense

            if (occludeeToRegister == null)
            {
                Debug.LogError($"Unable to register - given {nameof(occludeeToRegister)} is null.", this);
                return;
            }

            if (occludeeToRegister.Config == null)
            {
                Debug.LogError($"Unable to register - given Occludee's config is null.", this);
                return;
            }

            if (registeredOccludees.Contains(occludeeToRegister))
            {
                Debug.LogWarning($"Unable to register '{occludeeToRegister.name}' - Occludee has already been registered.", this);
                return;
            }

            #endregion

            if (config != null && rigRoot != null && config.IsModularClothingWorkflowEnabled)
            {
                ResolveBones(occludeeToRegister);
            }

            foreach (Occludee registeredOccludee in registeredOccludees)
            {
                registeredOccludee.Cull(occludeeToRegister);
                occludeeToRegister.Cull(registeredOccludee);
            }

            registeredOccludees.Add(occludeeToRegister);
        }

        public void Deregister(Occludee occludeeToDeregister)
        {
            #region Defense

            if (occludeeToDeregister == null)
            {
                Debug.LogError($"Unable to deregister - given {nameof(occludeeToDeregister)} is null.", this);
                return;
            }

            if (!registeredOccludees.Remove(occludeeToDeregister))
            {
                Debug.LogWarning($"Unable to deregister '{occludeeToDeregister.name}' - Occludee is not registered.", this);
                return;
            }

            #endregion

            if (config != null && rigRoot != null && config.IsModularClothingWorkflowEnabled)
            {
                ClearBones(occludeeToDeregister);
            }

            foreach (Occludee registeredOccludee in registeredOccludees)
            {
                registeredOccludee.Uncull(occludeeToDeregister);
                occludeeToDeregister.Uncull(registeredOccludee);
            }
        }

        #endregion
    }
}