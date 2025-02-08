using UnityEngine;
/*
public class Painter : MonoBehaviour
{
    public Color paintColor = Color.red;
    public int brushSize = 5;

    void Update()
    {
        if (Input.GetMouseButton(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Paintable"))
                {
                    PaintSurface(hit.textureCoord, hit.collider.gameObject);
                }
            }
        }
    }

    void PaintSurface(Vector2 uv, GameObject surface)
    {
        Renderer renderer = surface.GetComponent<Renderer>();
        Texture2D texture = renderer.material.mainTexture as Texture2D;

        int x = (int)(uv.x * texture.width);
        int y = (int)(uv.y * texture.height);

        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                if (x + i >= 0 && x + i < texture.width && y + j >= 0 && y + j < texture.height)
                {
                    texture.SetPixel(x + i, y + j, paintColor);
                }
            }
        }

        texture.Apply();
    }
}*/