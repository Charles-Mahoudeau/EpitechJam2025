using UnityEngine;
using System.Collections.Generic;
public class Painter : MonoBehaviour, IEquippable
{
    [Tooltip("How much the pixel color moves toward the target per second (in 0-1 range).")]
    [SerializeField] private float strength = 0.5f;  
   
    public Color paintColor = Color.red;
    public int brushSize = 5;
    private bool isEquipped = false;

    private Dictionary<GameObject, Texture2D> textureCopies = new Dictionary<GameObject, Texture2D>();
    public void Equip()
    {
        isEquipped = true;
    
    }

    public void Unequip()
    {
        isEquipped = false;
    
    }

    void Update()
    {
        if (!isEquipped) return;

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Paintable"))
                {
                    Debug.Log("Paintin' away: " + hit.collider.name);
                    PaintSurface(hit.textureCoord, hit.collider.gameObject);
                }
            }
        }
    }

    void PaintSurface(Vector2 uv, GameObject surface)
    {
        Renderer renderer = surface.GetComponent<Renderer>();

        if (textureCopies.TryGetValue(surface, out Texture2D existingTexture))
        {
            // Paint on existing texture
            PaintOnTexture(uv, existingTexture);
        }
        else
        {
            // Copy the original texture
            Texture2D originalTexture = renderer.material.mainTexture as Texture2D;
            Texture2D copiedTexture = new Texture2D(
                originalTexture.width,
                originalTexture.height,
                originalTexture.format,
                false
            );
            copiedTexture.SetPixels(originalTexture.GetPixels());
            copiedTexture.Apply();

            // Assign the copied texture to a new material
            Material materialInstance = new Material(renderer.material);
            materialInstance.mainTexture = copiedTexture;
            renderer.material = materialInstance;

            // Track the copied texture
            textureCopies.Add(surface, copiedTexture);

            PaintOnTexture(uv, copiedTexture);
        }
    }
    void PaintOnTexture(Vector2 uv, Texture2D texture)
    {
         Debug.Log("Paintin texture: " + texture.name);
        int centerX = (int)(uv.x * texture.width);
        int centerY = (int)(uv.y * texture.height);

        float step = strength * Time.deltaTime;

        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                
                if (i * i + j * j <= brushSize * brushSize)
                {
                    int x = centerX + i;
                    int y = centerY + j;
                    if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                    {
                        Color currentColor = texture.GetPixel(x, y);

                        float newR = Mathf.MoveTowards(currentColor.r, paintColor.r, step);
                        float newG = Mathf.MoveTowards(currentColor.g, paintColor.g, step);
                        float newB = Mathf.MoveTowards(currentColor.b, paintColor.b, step);
                        float newA = Mathf.MoveTowards(currentColor.a, paintColor.a, step);

                        Color newColor = new Color(newR, newG, newB, newA);
                        texture.SetPixel(x, y, newColor);
                    }
                }
            }
        }
        texture.Apply();
    }
}