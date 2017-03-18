namespace ImageSharp.Processing.Processors
{
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    /// <content>
    /// Conains the definition of <see cref="WeightsWindow"/> and <see cref="WeightsBuffer"/>.
    /// </content>
    internal abstract partial class ResamplingWeightedProcessor<TColor>
    {
        /// <summary>
        /// Points to a collection of of weights allocated in <see cref="WeightsBuffer"/>.
        /// </summary>
        protected unsafe struct WeightsWindow
        {
            /// <summary>
            /// The local left index position
            /// </summary>
            public int Left;

            /// <summary>
            /// The span of weights pointing to <see cref="WeightsBuffer"/>.
            /// </summary>
            public BufferSpan<float> Span;

            /// <summary>
            /// Initializes a new instance of the <see cref="WeightsWindow"/> struct.
            /// </summary>
            /// <param name="left">The local left index</param>
            /// <param name="span">The span</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal WeightsWindow(int left, BufferSpan<float> span)
            {
                this.Left = left;
                this.Span = span;
            }

            /// <summary>
            /// Gets an unsafe float* pointer to the beginning of <see cref="Span"/>.
            /// </summary>
            public float* Ptr => (float*)this.Span.PointerAtOffset;

            /// <summary>
            /// Gets the lenghth of the weights window
            /// </summary>
            public int Length => this.Span.Length;

            /// <summary>
            /// Computes the sum of vectors in 'rowSpan' weighted by weight values, pointed by this <see cref="WeightsWindow"/> instance.
            /// </summary>
            /// <param name="rowSpan">The input span of vectors</param>
            /// <returns>The weighted sum</returns>
            public Vector4 ComputeWeightedRowSum(BufferSpan<Vector4> rowSpan)
            {
                float* horizontalValues = this.Ptr;
                int left = this.Left;

                // Destination color components
                Vector4 result = Vector4.Zero;

                for (int i = 0; i < this.Length; i++)
                {
                    float xw = horizontalValues[i];
                    int index = left + i;
                    result += rowSpan[index] * xw;
                }

                return result;
            }

            /// <summary>
            /// Computes the sum of vectors in 'firstPassPixels' at a column pointed by 'x',
            /// weighted by weight values, pointed by this <see cref="WeightsWindow"/> instance.
            /// </summary>
            /// <param name="firstPassPixels">The buffer of input vectors in row first order</param>
            /// <param name="x">The column position</param>
            /// <returns>The weighted sum</returns>
            public Vector4 ComputeWeightedColumnSum(PinnedImageBuffer<Vector4> firstPassPixels, int x)
            {
                float* verticalValues = this.Ptr;
                int left = this.Left;

                // Destination color components
                Vector4 result = Vector4.Zero;

                for (int i = 0; i < this.Length; i++)
                {
                    float yw = verticalValues[i];
                    int index = left + i;
                    result += firstPassPixels[x, index] * yw;
                }

                return result;
            }
        }

        /// <summary>
        /// Holds the <see cref="WeightsWindow"/> values in an optimized contigous memory region.
        /// </summary>
        protected class WeightsBuffer : IDisposable
        {
            private PinnedImageBuffer<float> dataBuffer;

            /// <summary>
            /// Initializes a new instance of the <see cref="WeightsBuffer"/> class.
            /// </summary>
            /// <param name="sourceSize">The size of the source window</param>
            /// <param name="destinationSize">The size of the destination window</param>
            public WeightsBuffer(int sourceSize, int destinationSize)
            {
                this.dataBuffer = new PinnedImageBuffer<float>(sourceSize, destinationSize);
                this.dataBuffer.Clear();
                this.Weights = new WeightsWindow[destinationSize];
            }

            /// <summary>
            /// Gets the calculated <see cref="Weights"/> values.
            /// </summary>
            public WeightsWindow[] Weights { get; }

            /// <summary>
            /// Disposes <see cref="WeightsBuffer"/> instance releasing it's backing buffer.
            /// </summary>
            public void Dispose()
            {
                this.dataBuffer.Dispose();
            }

            /// <summary>
            /// Slices a weights value at the given positions.
            /// </summary>
            /// <param name="destIdx">The index in destination buffer</param>
            /// <param name="leftIdx">The local left index value</param>
            /// <param name="rightIdx">The local right index value</param>
            /// <returns>The weights</returns>
            public WeightsWindow GetWeightsWindow(int destIdx, int leftIdx, int rightIdx)
            {
                BufferSpan<float> span = this.dataBuffer.GetRowSpan(destIdx).Slice(leftIdx, rightIdx - leftIdx);
                return new WeightsWindow(leftIdx, span);
            }
        }
    }
}