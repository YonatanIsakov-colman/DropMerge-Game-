using EventBusScripts;
using UnityEngine;
using Event = EventBusScripts.Event;

namespace Events
{
    public class KeyboardMoveEvent:Event<Vector2> { }
    public class MouseStartDragEvent:Event<Vector2> { }
    public class MouseDraggingEvent:Event<Vector2> { }
    public class MouseStopDragEvent:Event { }
    public class KeyboardDropEvent:Event { }
}