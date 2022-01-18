using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Utils;
using UnityEngine;
using  Project.Scripts.Fractures;

namespace Project.Scripts.Fractures
{
    public static class Fracture
    {
        public static Dictionary<FractureThis,List<ChunkGraphManager> > cmDIC=new Dictionary<FractureThis, List<ChunkGraphManager>>();
        public static ChunkGraphManager FractureGameObject(FractureThis fs,GameObject gameObject, Anchor anchor, int seed, int totalChunks,Material insideMaterial, Material outsideMaterial, float jointBreakForce, float density)
        {
            // Translate all meshes to one world mesh
            var mesh = GetWorldMesh(gameObject);
            
            NvBlastExtUnity.setSeed(seed);

            var nvMesh = new NvMesh(
                mesh.vertices,
                mesh.normals,
                mesh.uv,
                mesh.vertexCount,
                mesh.GetIndices(0),
                (int) mesh.GetIndexCount(0)
            );
 float t3=Time.realtimeSinceStartup;

            var meshes = FractureMeshesInNvblast(totalChunks, nvMesh);
 float t4=Time.realtimeSinceStartup;
 Debug.Log(t4-t3+"   ---4444444444444444333333333333333");
            // Build chunks gameobjects
            var chunkMass = mesh.Volume() * density / totalChunks;
            var chunks = BuildChunks(fs,insideMaterial, outsideMaterial, meshes, chunkMass);
            
            // Connect blocks that are touching with fixed joints
            float t=Time.realtimeSinceStartup;
           foreach (var chunk in chunks)
            {
              ConnectTouchingChunks(chunk, jointBreakForce,0.01f,fs.layer);
            }
 float t2=Time.realtimeSinceStartup;
 Debug.Log(t2-t+"   ---");
            // Set anchored chunks as kinematic
        //    AnchorChunks(gameObject, anchor);

            var fractureGameObject = new GameObject("Fracture");
             var graphManager = fractureGameObject.AddComponent<ChunkGraphManager>();
             graphManager.nodes=chunks.ToArray();
            foreach (var chunk in chunks)
            {
                chunk.transform.SetParent(fractureGameObject.transform);
            }
            // Graph manager freezes/unfreezes blocks depending on whether they are connected to the graph or not
           
            graphManager.fs=fs;
            graphManager.Setup();
            
            return graphManager;
        }

        private static void AnchorChunks(GameObject gameObject, Anchor anchor)
        {
            var transform = gameObject.transform;
            var bounds = gameObject.GetCompositeMeshBounds();
            var anchoredColliders = GetAnchoredColliders(anchor, transform, bounds);
            
            foreach (var collider in anchoredColliders)
            {
                var chunkRb = collider.GetComponent<Rigidbody>();
                if (chunkRb)
                {
                   // chunkRb.isKinematic = true;
                }
            }
        }

        private static List<ChunkNode> BuildChunks(FractureThis fs,Material insideMaterial, Material outsideMaterial, List<Mesh> meshes, float chunkMass)
        {
            return meshes.Select((chunkMesh, i) =>
            {
                var chunk = BuildChunk(fs,insideMaterial, outsideMaterial, chunkMesh, chunkMass);
               chunk.name += $" [{i}]";
                return chunk;
            }).ToList();
        }

        private static List<Mesh> FractureMeshesInNvblast(int totalChunks, NvMesh nvMesh)
        {
            var fractureTool = new NvFractureTool();
            fractureTool.setRemoveIslands(false);
            fractureTool.setSourceMesh(nvMesh);
            var sites = new NvVoronoiSitesGenerator(nvMesh);
            sites.uniformlyGenerateSitesInMesh(totalChunks);
            fractureTool.voronoiFracturing(0, sites);
            fractureTool.finalizeFracturing();

            // Extract meshes
            var meshCount = fractureTool.getChunkCount();
            var meshes = new List<Mesh>(fractureTool.getChunkCount());
          //  Debug.Log(meshCount);
            for (var i = 1; i < meshCount; i++)
            {
                meshes.Add(ExtractChunkMesh(fractureTool, i));
            }

            return meshes;
        }

        private static IEnumerable<Collider> GetAnchoredColliders(Anchor anchor, Transform meshTransform, Bounds bounds)
        {
            var anchoredChunks = new HashSet<Collider>();
            var frameWidth = .01f;
            var meshWorldCenter = meshTransform.TransformPoint(bounds.center);
            var meshWorldExtents = bounds.extents.Multiply(meshTransform.lossyScale);
            
            if (anchor.HasFlag(Anchor.Left))
            {
                var center = meshWorldCenter - meshTransform.right * meshWorldExtents.x;
                var halfExtents = meshWorldExtents.Abs().SetX(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center, halfExtents, meshTransform.rotation));
            }
            
            if (anchor.HasFlag(Anchor.Right))
            {
                var center = meshWorldCenter + meshTransform.right * meshWorldExtents.x;
                var halfExtents = meshWorldExtents.Abs().SetX(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center, halfExtents, meshTransform.rotation));
            }
            
            if (anchor.HasFlag(Anchor.Bottom))
            {
                var center = meshWorldCenter - meshTransform.up * meshWorldExtents.y;
                var halfExtents = meshWorldExtents.Abs().SetY(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center, halfExtents, meshTransform.rotation));
            }
            
            if (anchor.HasFlag(Anchor.Top))
            {
                var center = meshWorldCenter + meshTransform.up * meshWorldExtents.y;
                var halfExtents = meshWorldExtents.Abs().SetY(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center, halfExtents, meshTransform.rotation));
            }
            
            if (anchor.HasFlag(Anchor.Front))
            {
                var center = meshWorldCenter - meshTransform.forward * meshWorldExtents.z;
                var halfExtents = meshWorldExtents.Abs().SetZ(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center,  halfExtents, meshTransform.rotation));
            }
            
            if (anchor.HasFlag(Anchor.Back))
            {
                var center = meshWorldCenter + meshTransform.forward * meshWorldExtents.z;
                var halfExtents = meshWorldExtents.Abs().SetZ(frameWidth);
                anchoredChunks.UnionWith(Physics.OverlapBox(center,  halfExtents, meshTransform.rotation));
            }
            
            return anchoredChunks;
        }

        private static Mesh ExtractChunkMesh(NvFractureTool fractureTool, int index)
        {
            var outside = fractureTool.getChunkMesh(index, false);
            var inside = fractureTool.getChunkMesh(index, true);
            var chunkMesh = outside.toUnityMesh();
            chunkMesh.subMeshCount = 2;
            chunkMesh.SetIndices(inside.getIndexes(), MeshTopology.Triangles, 1);
            return chunkMesh;
        }

        private static Mesh GetWorldMesh(GameObject gameObject)
        {
            var combineInstances = gameObject
                .GetComponentsInChildren<MeshFilter>()
                .Where(mf => ValidateMesh(mf.mesh))
                .Select(mf => new CombineInstance()
                {
                    mesh = mf.mesh,
                    transform = mf.transform.localToWorldMatrix
                }).ToArray();
            
            var totalMesh = new Mesh();
            totalMesh.CombineMeshes(combineInstances, true);
            return totalMesh;
        }
        
        private static bool ValidateMesh(Mesh mesh)
        {
            if (mesh.isReadable == false)
            {
                Debug.LogError($"Mesh [{mesh}] has to be readable.");
                return false;
            }
            
            if (mesh.vertices == null || mesh.vertices.Length == 0)
            {
                Debug.LogError($"Mesh [{mesh}] does not have any vertices.");
                return false;
            }
            
            if (mesh.uv == null || mesh.uv.Length == 0)
            {
                Debug.LogError($"Mesh [{mesh}] does not have any uvs.");
                return false;
            }

            return true;
        }

        private static ChunkNode BuildChunk( FractureThis fs,Material insideMaterial, Material outsideMaterial, Mesh mesh, float mass)
        {
            var chunk = new GameObject($"Chunk");
            var chunk2 = chunk.AddComponent<ChunkNode>();
            chunk2.render = chunk.AddComponent<MeshRenderer>();
          //  renderer.sharedMaterials = new[]
         //   {
          //      outsideMaterial,
          //      insideMaterial
         //   };

              chunk2.render .sharedMaterials =fs.outmat;

          chunk2.mf = chunk.AddComponent<MeshFilter>();
              chunk2.mf .sharedMesh = mesh;

           chunk2.rb = chunk.AddComponent<Rigidbody>();
             chunk2.rb.mass = mass;

             chunk2.col = chunk.AddComponent<MeshCollider>();
             chunk2.col.inflateMesh = true;
             chunk2.col.convex = true;
            
            return chunk2;
        }
       //  static HashSet<Rigidbody> overlaps = new HashSet<Rigidbody>(128);
           static   HashSet<ChunkNode> overlapsChunk = new HashSet<ChunkNode>(128);




        private static void ConnectTouchingChunks(ChunkNode chunk, float jointBreakForce, float touchRadius ,LayerMask layer)
        {
           // var rb = chunk.rb;
            var mesh = chunk.mf.mesh;
   // overlaps.Clear();
          overlapsChunk.Clear();
            var vertices = mesh.vertices;
            for (var i = 0; i < vertices.Length; i++)
            {
                var worldPosition = chunk.transform.TransformPoint(vertices[i]);
                var hits = Physics.OverlapSphere(worldPosition, touchRadius,layer);
                for (var j = 0; j < hits.Length; j++)
                {
                    ChunkNode node=hits[j].GetComponent<ChunkNode>();
                  
                    
                         overlapsChunk.Add(node);
                  
                   
                }
            }

         
            overlapsChunk.Remove(chunk);
          
                chunk.Neighbours=overlapsChunk.ToArray();
        }
    }
}