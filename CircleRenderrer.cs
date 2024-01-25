using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
public static class CircleRenderrerTests
{
	public delegate Bitmap CircleRenderrer(ushort pixelRadius, Color circleColor, Color backgroundColor);
	public static void RunAllTests(CircleRenderrer canidate, CircleRenderrer knownMethod)
	{

	}
	public static void TestAccuracy(CircleRenderrer canidate, CircleRenderrer knownMethod)
	{

	}
	public static void TestSpeed(CircleRenderrer canidate, CircleRenderrer knownMethod, params int[] testDiameters = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 32, 64, 128, 256, 512 })
	{

	}
	public static void GenerateWorkSamples()
	{

	}
}
public static class CircleRenderrers
{
	public const int SpeedTestDiameter = 512;
	public const int SpeedTestCount = 25;
	public static int[] AccuracyTestDiameters = ;
	public const int WorkSampleDiameter = 512;
	public static readonly Random RNG = new Random((int)DateTime.Now.Ticks);
	public static readonly Color CircleColor = Color.FromArgb(255, 245, 150, 10);
	public static readonly Color BackgroundColor = Color.FromArgb(255, 0, 200, 250);
	public static void RunAllTests()
	{
		Console.WriteLine("Testing accuracy...");
		for (int i = 0; i < AccuracyTestDiameters.Length; i++)
		{
			int accuracyTestDiameter = AccuracyTestDiameters[i];
			Bitmap singlePixelMethodOutput = RenderCircleSlow(accuracyTestDiameter, CircleColor, BackgroundColor);
			Bitmap rowCloneMethodOutput = RenderCircleFast(accuracyTestDiameter, CircleColor, BackgroundColor);
			Bitmap GDIPlusMethodOutput = RenderMyBall(accuracyTestDiameter, CircleColor, BackgroundColor);
			if (!BitmapsEqual(singlePixelMethodOutput, rowCloneMethodOutput))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"ERROR: rowCloneMethod was wrong for a diameter of {accuracyTestDiameter}.");
				Console.ForegroundColor = ConsoleColor.White;
			}
			singlePixelMethodOutput.Dispose();
			rowCloneMethodOutput.Dispose();
			GDIPlusMethodOutput.Dispose();
		}
		Console.WriteLine("Tested accuracy.");
		
		Console.WriteLine();

		Console.WriteLine("Testing speed...");
		Stopwatch stopwatch = Stopwatch.StartNew();
		long slowStartTicks = stopwatch.ElapsedTicks;
		for (int i = 0; i < SpeedTestCount; i++)
		{
			Bitmap output = RenderCircleSlow(SpeedTestDiameter, CircleColor, BackgroundColor);
			output.Dispose();
		}
		long slowEndTicks = stopwatch.ElapsedTicks;
		long slowWayTicks = (slowEndTicks - slowStartTicks);

		long fastStartTicks = stopwatch.ElapsedTicks;
		for (int i = 0; i < SpeedTestCount; i++)
		{
			Bitmap output = RenderCircleFast(SpeedTestDiameter, CircleColor, BackgroundColor);
			output.Dispose();
		}
		long fastEndTicks = stopwatch.ElapsedTicks;
		long fastWayTicks = (fastEndTicks - fastStartTicks);

		Console.WriteLine($"Slow method took {slowWayTicks} ticks.");
		Console.WriteLine($"Fast method took {fastWayTicks} ticks.");
		Console.WriteLine($"Which makes the fast way {slowWayTicks - fastWayTicks} ticks faster or {slowWayTicks / (double)fastWayTicks} times faster.");
		Console.WriteLine("Tested speed.");
		Console.WriteLine();
		
		Console.WriteLine("Generating samples...");
		Bitmap slowWorkSample = RenderCircleSlow(WorkSampleDiameter, CircleColor, BackgroundColor);
		slowWorkSample.Save("SlowMethodSample.png");
		slowWorkSample.Dispose();
		Bitmap fastWorkSample = RenderCircleFast(WorkSampleDiameter, CircleColor, BackgroundColor);
		fastWorkSample.Save("FastMethodSample.png");
		fastWorkSample.Dispose();
		Bitmap fastestWorkSample = RenderMyBall(WorkSampleDiameter, CircleColor, BackgroundColor);
		fastestWorkSample.Save("FastestMethodSample.png");
		fastestWorkSample.Dispose();
		Console.WriteLine("Generated samples.");
		
		Console.WriteLine();
		ExitOnKeyPress();
	}
	public static bool ColorsEqual(Color a, Color b)
	{
		return a.A == b.A && a.R == b.R && a.G == b.G && a.B == b.B;
	}
	public static bool BitmapsEqual(Bitmap a, Bitmap b)
	{
		if (a is null && b is null)
		{
			return true;
		}
		else if (a is null || b is null)
		{
			return false;
		}
		else if (a.Width != b.Width || a.Height != b.Height)
		{
			return false;
		}
		else
		{
			for (int x = 0; x < a.Width; x++)
			{
				for (int y = 0; y < a.Height; y++)
				{
					if (!ColorsEqual(a.GetPixel(x, y), b.GetPixel(x, y)))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
	public static void ExitOnKeyPress()
	{
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine("Press any key to exit...");
		while (true)
		{
			long promptTime = DateTime.Now.Ticks;
			Console.ReadKey(true);
			if (DateTime.Now.Ticks - promptTime > 1000000)
			{
				break;
			}
		}
		Process.GetCurrentProcess().Kill();
	}
	//Big O of n squared.
	public static Bitmap RenderCircleSlow(int pixelDiameter, Color circleColor, Color backgroundColor)
	{
		if (pixelDiameter < 1)
		{
			throw new Exception("pixelDiameter must be greater than 0.");
		}
		Bitmap output = new Bitmap(pixelDiameter, pixelDiameter);
		double centerX = pixelDiameter / 2.0;
		double centerY = pixelDiameter / 2.0;
		double radius = pixelDiameter / 2.0;
		for (int x = 0; x < pixelDiameter; x++)
		{
			for (int y = 0; y < pixelDiameter; y++)
			{
				double scaledX = x + 0.5;
				double scaledY = y + 0.5;
				double distanceX = scaledX - centerX;
				double distanceY = scaledY - centerY;
				double distance = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
				if (distance < radius)
				{
					output.SetPixel(x, y, circleColor);
				}
				else
				{
					output.SetPixel(x, y, backgroundColor);
				}
			}
		}
		return output;
	}
	// Big O of one half n - 1.
	public static Bitmap RenderCircleFast(int pixelDiameter, Color circleColor, Color backgroundColor)
	{
		if (pixelDiameter < 1)
		{
			throw new Exception("pixelDiameter must be greater than 0.");
		}
		else if (pixelDiameter is 1)
		{
			if (BitConverter.IsLittleEndian)
			{
				byte[] buffer = new byte[4] { circleColor.B, circleColor.G, circleColor.R, circleColor.A };
				return new Bitmap(1, 1, 4, PixelFormat.Format32bppArgb, Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0));
			}
			else
			{
				byte[] buffer = new byte[4] { circleColor.A, circleColor.R, circleColor.G, circleColor.B };
				return new Bitmap(1, 1, 4, PixelFormat.Format32bppArgb, Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0));
			}
		}
		else
		{
			int halfStride = pixelDiameter << 1;
			int stride = halfStride << 1;
			byte[] buffer = new byte[stride * pixelDiameter];
			byte[] strideOfCircleColor = new byte[stride];
			byte[] halfStrideOfBackgroundColor = new byte[halfStride];
			if (BitConverter.IsLittleEndian)
			{
				strideOfCircleColor[0] = circleColor.B;
				strideOfCircleColor[1] = circleColor.G;
				strideOfCircleColor[2] = circleColor.R;
				strideOfCircleColor[3] = circleColor.A;
				halfStrideOfBackgroundColor[0] = backgroundColor.B;
				halfStrideOfBackgroundColor[1] = backgroundColor.G;
				halfStrideOfBackgroundColor[2] = backgroundColor.R;
				halfStrideOfBackgroundColor[3] = backgroundColor.A;
			}
			else
			{
				strideOfCircleColor[0] = circleColor.A;
				strideOfCircleColor[1] = circleColor.R;
				strideOfCircleColor[2] = circleColor.G;
				strideOfCircleColor[3] = circleColor.B;
				halfStrideOfBackgroundColor[0] = backgroundColor.A;
				halfStrideOfBackgroundColor[1] = backgroundColor.R;
				halfStrideOfBackgroundColor[2] = backgroundColor.G;
				halfStrideOfBackgroundColor[3] = backgroundColor.B;
			}
			int filledSectionLength = 4;
			while (filledSectionLength <= halfStride)
			{
				Array.Copy(strideOfCircleColor, 0, strideOfCircleColor, filledSectionLength, filledSectionLength);
				filledSectionLength <<= 1;
			}
			if (filledSectionLength != stride)
			{
				Array.Copy(strideOfCircleColor, 0, strideOfCircleColor, filledSectionLength, stride - filledSectionLength);
			}
			filledSectionLength = 4;
			while (filledSectionLength <= pixelDiameter)
			{
				Array.Copy(halfStrideOfBackgroundColor, 0, halfStrideOfBackgroundColor, filledSectionLength, filledSectionLength);
				filledSectionLength <<= 1;
			}
			if (filledSectionLength != halfStride)
			{
				Array.Copy(halfStrideOfBackgroundColor, 0, halfStrideOfBackgroundColor, filledSectionLength, halfStride - filledSectionLength);
			}
			if ((pixelDiameter & 1) is 0)
			{
				int pixelRadius = pixelDiameter >> 1;
				int bufferPosition = pixelRadius * stride;
				int bufferPosition2 = bufferPosition - stride;
				Array.Copy(strideOfCircleColor, 0, buffer, bufferPosition, stride);
				bufferPosition += stride;
				Array.Copy(strideOfCircleColor, 0, buffer, bufferPosition2, stride);
				bufferPosition2 -= stride;
				for (int y = pixelRadius + 1; y < pixelDiameter; y++)
				{
					double rowHeight = y + 0.5;
					int layerWidth = ((int)(Math.Sqrt(rowHeight * (pixelDiameter - rowHeight)) + 0.5)) << 1;
					int layerByteWidth = layerWidth << 2;
					int boarderByteWidth = ((pixelDiameter - layerWidth) >> 1) << 2;
					int localBufferPosition = bufferPosition;
					Array.Copy(halfStrideOfBackgroundColor, 0, buffer, localBufferPosition, boarderByteWidth);
					localBufferPosition += boarderByteWidth;
					Array.Copy(strideOfCircleColor, 0, buffer, localBufferPosition, layerByteWidth);
					localBufferPosition += layerByteWidth;
					Array.Copy(halfStrideOfBackgroundColor, 0, buffer, localBufferPosition, boarderByteWidth);
					Array.Copy(buffer, bufferPosition, buffer, bufferPosition2, stride);
					bufferPosition += stride;
					bufferPosition2 -= stride;
				}
			}
			else
			{
				int pixelRadius = pixelDiameter >> 1;
				int bufferPosition = pixelRadius * stride;
				int bufferPosition2 = bufferPosition - stride;
				Array.Copy(strideOfCircleColor, 0, buffer, bufferPosition, stride);
				bufferPosition += stride;
				for (int y = pixelRadius + 1; y < pixelDiameter; y++)
				{
					double rowHeight = y + 0.5;
					int layerWidth = (((int)Math.Sqrt(rowHeight * (pixelDiameter - rowHeight))) << 1) + 1;
					int layerByteWidth = layerWidth << 2;
					int boarderByteWidth = ((pixelDiameter - layerWidth) >> 1) << 2;
					int localBufferPosition = bufferPosition;
					Array.Copy(halfStrideOfBackgroundColor, 0, buffer, localBufferPosition, boarderByteWidth);
					localBufferPosition += boarderByteWidth;
					Array.Copy(strideOfCircleColor, 0, buffer, localBufferPosition, layerByteWidth);
					localBufferPosition += layerByteWidth;
					Array.Copy(halfStrideOfBackgroundColor, 0, buffer, localBufferPosition, boarderByteWidth);
					Array.Copy(buffer, bufferPosition, buffer, bufferPosition2, stride);
					bufferPosition += stride;
					bufferPosition2 -= stride;
				}
			}
			return new Bitmap(pixelDiameter, pixelDiameter, stride, PixelFormat.Format32bppArgb, Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0));
		}
	}
	// Big O of literally 1.
	public static Bitmap RenderMyBall(int pixelDiameter, Color circleColor, Color backgroundColor)
	{
		Bitmap bitmap = new Bitmap(pixelDiameter, pixelDiameter);

		Graphics graphics = Graphics.FromImage(bitmap);

		graphics.Clear(backgroundColor);

		SolidBrush solidBrush = new SolidBrush(circleColor);

		graphics.FillEllipse(solidBrush, new Rectangle(0, 0, pixelDiameter, pixelDiameter));

		graphics.Dispose();

		return bitmap;
	}
}