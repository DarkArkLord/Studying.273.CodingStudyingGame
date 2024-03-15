﻿using Assets.Scripts.CommonComponents;
using UnityEngine;

namespace Assets.Scripts.States.Map.Components
{
    public class FloorElementsKeeper : MonoBehaviour
    {
        [SerializeField]
        private ObjectPoolComponent pathObjectPool;
        public ObjectPoolComponent PathObjectPool => pathObjectPool;

        [SerializeField]
        private ObjectPoolComponent wallObjectPool;
        public ObjectPoolComponent WallObjectPool => wallObjectPool;

        [SerializeField]
        private Material[] pathMaterials;
        public Material[] PathMaterials => pathMaterials;

        [SerializeField]
        private Material[] wallMaterials;
        public Material[] WallMaterials => wallMaterials;

        [SerializeField]
        private Material inputMaterial;
        public Material InputMaterial => inputMaterial;

        [SerializeField]
        private Material outputMaterial;
        public Material OutputMaterial => outputMaterial;

        [SerializeField]
        private Material noneMaterial;
        public Material NoneMaterial => noneMaterial;
    }
}
