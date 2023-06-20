namespace SpaceBattle.Scripts.Unit
{
	internal class TestScript
	{
		private int x;
		private int y;

		private void Start()
		{
			Translate(1,1);
			Function();
			TranslatePosition(1);

			DamageTarget(1.0f);

			Destroy();

			MethodWithLongDescription();

			MethodWithManyUses();
		}


		/// <summary>
		/// This method is so complicated that is can be used multiple ways
		/// <list type="bullet"> Text before the list
		/// <item>This is item number 1</item>
		/// <item>This is item number 2</item>
		/// <item>This is item number 3</item>
		/// </list>
		/// You can even add text after the list
		/// </summary>
		private void MethodWithManyUses()
		{
			
		}

		/// <summary>
		/// This method has so much to describe that it requires an entire additional paragraf.
		/// <para>
		/// This is the second paragraf which was described earlier.
		/// </para>
		/// </summary>
		private void MethodWithLongDescription()
		{

		}

		/// <summary>
		/// Destroys all objects attachted to this <see cref="TestScript"/>.
		/// </summary>
		private void Destroy()
		{

		}

		/// <summary>
		/// <see langword="this"/> target will take <paramref name="amount"/> in damage.
		/// </summary>
		/// <param name="amount"></param>
		private void DamageTarget(float amount)
		{

		}
		
		/// <summary>
		/// <inheritdoc cref="TestScript.DamageTarget(float)"></inheritdoc>
		/// </summary>
		/// <param name="i"></param>
		private void DamageTarget(int amount)
		{

		}

		/// <summary>This is the entry point of the Point class testing program.
		/// <para>
		/// This program tests each method and operator, and
		/// is intended to be run after any non-trivial maintenance has
		/// been performed on the Point class.
		/// </para>
		/// </summary>
		private static void Main()
		{

		}

		/// <summary>Here is an example of a bulleted list:
		/// <list type="bullet">
		/// <item>
		/// <description>Item 1.</description>
		/// </item>
		/// <item>
		/// <description>Item 2.</description>
		/// </item>
		/// </list>
		/// </summary>
		private void Function()
		{
			x = 1;
		}

		/// <summary>
		/// This method changes the point's location by the given x- and y-offsets.
		/// <example>
		/// Example:
		/// <code>
		/// Point p = new Point(3,5);
		/// p.Translate(-1,3);
		/// </code>
		/// results in <c>p</c>'s having the value (2,8).
		/// </example>
		/// </summary>
		private void Translate(int x, int y)
		{
			this.x += x;
			this.y += y;
		}

		/// <summary>This translates the objects position forward by <paramref name="value"/>.
		/// </summary>
		/// <param name="value">the amount <see langword="this"/> object is moved.</param>
		private void TranslatePosition(int value)
		{
			this.x += value;
			this.y += y;
		}

		/// <remarks>
		/// Uses polar coordinates
		/// </remarks>
		public static void Test()
		{
			// ...
		}
	}
}