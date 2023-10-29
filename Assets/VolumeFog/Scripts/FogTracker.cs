using UnityEngine;

public class FogTracker : MonoBehaviour

{
    public float updateInterval = 0.1f; // How often to update the texture

    public Texture3D dynamicTexture; // Assign your existing texture in the Unity Editor

    private float timer;

    void Start()
    {
        if (dynamicTexture == null)
        {
            Debug.LogError("Please assign a Texture3D to dynamicTexture in the Unity Editor.");
            return;
        }

        InitializeTexture();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            UpdatePlayerPositionInTexture();
            timer = 0f;
        }
    }

    void InitializeTexture()
    {
        int textureSize = dynamicTexture.width; // Assuming the texture is cubic

        // Ensure the texture is readable and is set to RGBA32 format
        dynamicTexture.filterMode = FilterMode.Bilinear;
        dynamicTexture.wrapMode = TextureWrapMode.Clamp;

        // Initialize the texture with a default color (e.g., white)
        Color[] colors = new Color[textureSize * textureSize * textureSize];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }

        dynamicTexture.SetPixels(colors);
        dynamicTexture.Apply();
    }

    void UpdatePlayerPositionInTexture()
    {
        int textureSize = dynamicTexture.width;

        Vector3 playerPosition = transform.position;
        Vector3 normalizedPosition = new Vector3(
            playerPosition.x / textureSize,
            playerPosition.y / textureSize,
            playerPosition.z / textureSize
        );

        // Convert normalized position to texture coordinates
        int x = Mathf.FloorToInt(normalizedPosition.x * (textureSize - 1));
        int y = Mathf.FloorToInt(normalizedPosition.y * (textureSize - 1));
        int z = Mathf.FloorToInt(normalizedPosition.z * (textureSize - 1));

        // Set the player's position as black in the texture
        Color[] pixels = dynamicTexture.GetPixels();
        int index = x + y * textureSize + z * textureSize * textureSize;
        pixels[index] = Color.black;
        dynamicTexture.SetPixels(pixels);
        dynamicTexture.Apply();
    }
}