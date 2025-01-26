using SDL;

namespace ProjectTerra.Framework.Maths;

public static class Time{
    private static float _lastFrameTime;

    public static float deltaTime(){
        float curTime = SDL3.SDL_GetTicks() / 1000;
        float deltaTime = curTime - _lastFrameTime;
        _lastFrameTime = curTime;
        return deltaTime;
    }
}