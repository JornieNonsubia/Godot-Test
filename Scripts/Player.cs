using Godot;

public partial class Player : CharacterBody3D
{
	public const float walkSpeed = 5.0f;
	public const float sprintSpeed = 8.0f;
	public const float jumpVelocity = 4.5f;
	public float mouseSens = 0.1f;
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	private float curSpeed;
	private Node3D _head;
	private Node3D _pistol;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		_head = this.GetNode<Node3D>("Head");
	}

	//Camera control
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion m)
		{
			RotateY(-Mathf.DegToRad(m.Relative.X * mouseSens));
			_head.RotateX(-Mathf.DegToRad(m.Relative.Y * mouseSens));

			Vector3 headRot = _head.Rotation;
			headRot.X = Mathf.Clamp(headRot.X, Mathf.DegToRad(-80f), Mathf.DegToRad(80f));
			_head.Rotation = headRot;


		}
		if (@event is InputEventMouseButton)
		{
			GD.Print(@event);
		}
	}

	//Move Control
	public override void _PhysicsProcess(double delta)

	{
		Vector3 velocity = Velocity;

		if (Input.IsKeyPressed(Key.Escape))
			GetTree().Quit();

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
			velocity.Y = jumpVelocity;


		if (Input.IsActionPressed("sprint"))
			curSpeed = sprintSpeed;
		else
			curSpeed = walkSpeed;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * curSpeed;
			velocity.Z = direction.Z * curSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, curSpeed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, curSpeed);
		}

		Velocity = velocity;
		MoveAndSlide();


	}

}
