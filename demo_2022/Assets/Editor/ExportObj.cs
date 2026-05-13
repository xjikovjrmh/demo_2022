using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class ExportObj : EditorWindow
{
    [MenuItem("Tools/Export Selected Mesh to OBJ")]
    static void ExportToObj()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.LogError("请先在 Hierarchy 里选中一个物体");
            return;
        }

        // 收集所有子物体的 MeshFilter
        MeshFilter[] meshFilters = selected.GetComponentsInChildren<MeshFilter>();

        if (meshFilters.Length == 0)
        {
            Debug.LogError("选中的物体及其子物体没有 MeshFilter，无法导出");
            return;
        }

        // 生成 .obj 文件内容
        StringBuilder objContent = new StringBuilder();
        int vertexOffset = 0;

        objContent.AppendLine("# Exported from Unity");
        objContent.AppendLine($"# Object: {selected.name}");

        foreach (MeshFilter mf in meshFilters)
        {
            Mesh mesh = mf.sharedMesh;
            if (mesh == null) continue;

            string objectName = mf.gameObject.name;
            objContent.AppendLine($"\no {objectName}");

            // 顶点
            foreach (Vector3 v in mesh.vertices)
            {
                Vector3 pos = mf.transform.TransformPoint(v);
                objContent.AppendLine($"v {pos.x:F6} {pos.y:F6} {pos.z:F6}");
            }

            // 法线
            foreach (Vector3 n in mesh.normals)
            {
                Vector3 normal = mf.transform.TransformDirection(n);
                objContent.AppendLine($"vn {normal.x:F6} {normal.y:F6} {normal.z:F6}");
            }

            // UV
            foreach (Vector2 uv in mesh.uv)
            {
                objContent.AppendLine($"vt {uv.x:F6} {uv.y:F6}");
            }

            // 面（三角面）
            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                int[] triangles = mesh.GetTriangles(i);
                for (int j = 0; j < triangles.Length; j += 3)
                {
                    int a = triangles[j] + vertexOffset + 1;
                    int b = triangles[j + 1] + vertexOffset + 1;
                    int c = triangles[j + 2] + vertexOffset + 1;
                    objContent.AppendLine($"f {a}/{a}/{a} {b}/{b}/{b} {c}/{c}/{c}");
                }
            }

            vertexOffset += mesh.vertices.Length;
        }

        // 保存文件
        string path = EditorUtility.SaveFilePanel("导出 OBJ", "", selected.name + ".obj", "obj");
        if (string.IsNullOrEmpty(path)) return;

        File.WriteAllText(path, objContent.ToString());
        Debug.Log($"已导出到: {path}");

        // 自动刷新 Asset 数据库
        AssetDatabase.Refresh();
    }
}