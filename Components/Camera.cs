#pragma warning disable IDE1006
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using ProjectTerra.Framework.Graphics;

namespace ProjectTerra.Framework.Components;

class Camera : GameObject {
    public Matrix4 model { get; private set; } = Matrix4.Identity;
    public Matrix4 view;
    public Matrix4 projection;

    public float FieldOfView { get; set; } = 45f; // fov aka that thing which zooms your screen out because its cool
    public float aspectRatio { get; set; } = 16f / 9f;
    public float nearPlane { get; set; } = 0.1f;
    public float farPlane { get; set; } = 100f;

    private Vector3 _target = Vector3.UnitZ;
    private Vector3 _orientation = Vector3.UnitY;

    private DefaultShader shader; // Required to update the view and projection matrices

    public Camera(string name, float aspectratio, DefaultShader shader) : base(name) {
        aspectRatio = aspectratio;
        this.name = name;
        this.shader = shader;
    }

    public override void Update(){
        projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FieldOfView), aspectRatio, nearPlane, farPlane);
        view = Matrix4.LookAt(transform.position, transform.position + _target, _orientation);
    }

    public override void Render(){
        base.Render();
        GL.UniformMatrix4(GL.GetUniformLocation(shader.GetProgram(), "view"), false, ref view);
        GL.UniformMatrix4(GL.GetUniformLocation(shader.GetProgram(), "projection"), false, ref projection);
    }

    //public void Move(Vector3 direction) => transform.Translate(direction);
    public void Move(Vector3 direction, float deltaTime) {
        float speed = 5.0f * deltaTime;
        transform.Translate(direction * speed);
    }

    public void Rotate(Vector3 direction) {
        transform.Rotate(direction);

        var yaw = transform.rotation.Y % 360;
        var pitch = Math.Clamp(transform.rotation.X, -89, 89);

        var target = new Vector3(
            MathF.Cos(MathHelper.DegreesToRadians(yaw)) * MathF.Cos(MathHelper.DegreesToRadians(pitch)),
            MathF.Sin(MathHelper.DegreesToRadians(pitch)),
            MathF.Sin(MathHelper.DegreesToRadians(yaw)) * MathF.Cos(MathHelper.DegreesToRadians(pitch))
        );

        _target = Vector3.Normalize(target);
    }
}
/*
    private void UpdateCamera(InputManager inputManager, Camera camera, float deltaTime){
        var keyboard = inputManager.GetActiveDevice<KeyboardMouseID>();
        if (keyboard != null)
        {
            Vector3 direction = Vector3.Zero;

            // Перемещение вперёд-назад
            if (keyboard.IsKeyDown(SDL_Scancode.SDL_SCANCODE_W))
                direction.Z -= 1.0f; // Камера движется вперёд
            if (keyboard.IsKeyDown(SDL_Scancode.SDL_SCANCODE_S))
                direction.Z += 1.0f; // Камера движется назад

            // Перемещение влево-вправо
            if (keyboard.IsKeyDown(SDL_Scancode.SDL_SCANCODE_A))
                direction.X -= 1.0f; // Камера движется влево
            if (keyboard.IsKeyDown(SDL_Scancode.SDL_SCANCODE_D))
                direction.X += 1.0f; // Камера движется вправо

            // Ускорение движения
            float speed = 5.0f * deltaTime; // Скорость перемещения
            camera.Move(direction * speed);

            // Обработка вращения камеры мышью
            var mouseDelta = keyboard.GetMousePos();
            float sensitivity = 0.1f;
            camera.Rotate(mouseDelta.x * sensitivity, mouseDelta.y * sensitivity);
        }
    }
*/