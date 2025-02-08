using UnityEngine;

public class Painter : MonoBehaviour, IEquippable
{
    [Tooltip("How much the pixel color moves toward the target per second (in 0-1 range).")]
    [SerializeField] private float strength = 0.5f;  
   
    public Color paintColor = Color.red;
    public int brushSize = 5;
    private bool isEquipped = false;

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
        
        if (renderer.material.name.StartsWith("Instance"))
        {
        
            Texture2D existingTexture = renderer.material.mainTexture as Texture2D;
            PaintOnTexture(uv, existingTexture);
        }
        else
        {
            Texture2D originalTexture = renderer.material.mainTexture as Texture2D;
            Texture2D copiedTexture = new Texture2D(
                originalTexture.width,
                originalTexture.height,
                originalTexture.format,
                false
            );
            copiedTexture.SetPixels(originalTexture.GetPixels());
            copiedTexture.Apply();
        
            Material materialInstance = new Material(renderer.material);
            materialInstance.name = "Instance_" + renderer.material.name;
            materialInstance.mainTexture = copiedTexture;
            renderer.material = materialInstance;

            PaintOnTexture(uv, copiedTexture);
        }
    }
    void PaintOnTexture(Vector2 uv, Texture2D texture)
    {
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