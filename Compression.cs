using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace CompressV2
{
    class Compression
    {
        static public void TxtToBinDouble(string pathIn, string pathOut)
        {
            StreamReader stmRdr = new StreamReader(File.Open(pathIn, FileMode.Open), Encoding.ASCII);
            BinaryWriter binWtr = new BinaryWriter(File.Open(pathOut, FileMode.Create), Encoding.ASCII);

            string line;
            while ((line = stmRdr.ReadLine()) != null)
            {
                double number = Convert.ToDouble(line);
                binWtr.Write(number);
            }

            stmRdr.Close();
            binWtr.Close();
        }

        static public void DoubleToShort(string pathIn, string pathOut,
            short newMin = short.MinValue, short newMax = short.MaxValue)
        {
            BinaryReader binRdr = new BinaryReader(File.Open(pathIn, FileMode.Open), Encoding.ASCII);
            BinaryWriter binWrtr = new BinaryWriter(File.Open(pathOut, FileMode.Create), Encoding.ASCII);
            //pathOut = pathOut.Replace(".bin", ".txt");
            //StreamWriter strmWrtr = new StreamWriter(pathOut);

            int length = binRdr.ReadInt32();
            byte miss = binRdr.ReadByte();
            byte beginWith = binRdr.ReadByte();
            binWrtr.Write(length);
            binWrtr.Write(miss);
            binWrtr.Write(beginWith);
            //strmWrtr.Write(Convert.ToString(length) + "\r\n");
            //strmWrtr.Write(Convert.ToString(miss) + "\r\n");
            //strmWrtr.Write(Convert.ToString(beginWith) + "\r\n");

            double maxValue = Double.MinValue, minValue = Double.MaxValue;
            //search for max and min
            do
            {
                double value = binRdr.ReadDouble();
                if (value > maxValue)
                    maxValue = value;
                if (value < minValue)
                    minValue = value;
            } while (binRdr.PeekChar() > -1);
            binWrtr.Write(minValue);
            binWrtr.Write(maxValue);
            //strmWrtr.Write(Convert.ToString(minValue) + "\r\n");
            //strmWrtr.Write(Convert.ToString(maxValue) + "\r\n");

            //into int16
            binRdr.BaseStream.Position = 0;
            binRdr.ReadInt32();
            binRdr.ReadByte();
            binRdr.ReadByte();
            do
            {
                double value = binRdr.ReadDouble();
                short x = (short)(newMin + (value - minValue) * (newMax - newMin) / (maxValue - minValue));
                binWrtr.Write(x);
                //strmWrtr.Write(Convert.ToString(x) + "\r\n");
            } while (binRdr.PeekChar() > -1);

            binRdr.Close();
            binWrtr.Close();
            //strmWrtr.Close();
        }

        static public void ShortToDouble(string pathIn, string pathOut)
        {
            BinaryReader binRdr = new BinaryReader(File.Open(pathIn, FileMode.Open), Encoding.ASCII);
            BinaryWriter binWrtr = new BinaryWriter(File.Open(pathOut, FileMode.Create), Encoding.ASCII);
            //pathOut = pathOut.Replace(".bin", ".txt");
            //StreamWriter strmWrtr = new StreamWriter(pathOut);

            double doubleMin = double.MaxValue, doubleMax = double.MinValue;

            int length = binRdr.ReadInt32();
            byte miss = binRdr.ReadByte();
            byte beginWith = binRdr.ReadByte();

            doubleMin = binRdr.ReadDouble();
            doubleMax = binRdr.ReadDouble();

            binWrtr.Write(length);
            binWrtr.Write(miss);
            binWrtr.Write(beginWith);
            //strmWrtr.Write(Convert.ToString(length) + "\r\n");
            //strmWrtr.Write(Convert.ToString(miss) + "\r\n");
            //strmWrtr.Write(Convert.ToString(beginWith) + "\r\n");

            short shortMin = short.MaxValue, shortMax = short.MinValue;
            do
            {
                short cur = binRdr.ReadInt16();
                if (cur < shortMin)
                    shortMin = cur;
                if (cur > shortMax)
                    shortMax = cur;
            } while(binRdr.PeekChar() > -1);
            binRdr.BaseStream.Position = 0;
            binRdr.ReadInt32();
            binRdr.ReadByte();
            binRdr.ReadByte();
            binRdr.ReadDouble();
            binRdr.ReadDouble();

            do
            {
                short cur = binRdr.ReadInt16();
                double x = doubleMin + (cur - shortMin) * (doubleMax - doubleMin) 
                    / (shortMax - shortMin);
                binWrtr.Write(x);
                //strmWrtr.Write(Convert.ToString(x) + "\r\n");
            } while(binRdr.PeekChar() > -1);

            binRdr.Close();
            binWrtr.Close();
            //strmWrtr.Close();
        }

        static public void DelShort(string pathIn, string pathOut)
        {
            BinaryReader binRdr = new BinaryReader(File.Open(pathIn, FileMode.Open), Encoding.ASCII);
            BinaryWriter binWrtr = new BinaryWriter(File.Open(pathOut, FileMode.Create), Encoding.ASCII);
            //pathOut = pathOut.Replace(".bin", ".txt");
            //StreamWriter strmWrtr = new StreamWriter(pathOut);

            int length = binRdr.ReadInt32(); //reand length
            byte miss = binRdr.ReadByte();
            byte beginWith = binRdr.ReadByte();
            double min = binRdr.ReadDouble(); //read min
            double max = binRdr.ReadDouble(); //read max
            binWrtr.Write(length);
            binWrtr.Write(miss);
            binWrtr.Write(beginWith);
            binWrtr.Write(min);
            binWrtr.Write(max);
            //strmWrtr.Write(Convert.ToString(length) + "\r\n");
            //strmWrtr.Write(Convert.ToString(miss) + "\r\n");
            //strmWrtr.Write(Convert.ToString(beginWith) + "\r\n");
            //strmWrtr.Write(Convert.ToString(min) + "\r\n");
            //strmWrtr.Write(Convert.ToString(max) + "\r\n");

            short last;
            last = binRdr.ReadInt16();
            binWrtr.Write(last);
            //strmWrtr.Write(Convert.ToString(last) + "\r\n");
            do
            {
                short cur = binRdr.ReadInt16();
                short del = (short)(cur - last);
                binWrtr.Write(del);
                //strmWrtr.Write(Convert.ToString(del) + "\r\n");
                last = cur;
            } while (binRdr.PeekChar() > -1);

            binRdr.Close();
            binWrtr.Close();
            //strmWrtr.Close();
        }

        static public void DelSmartOffset(string pathIn, string pathOut, byte bits = 4)
        {
            BinaryReader binRdr = new BinaryReader(File.Open(pathIn, FileMode.Open), Encoding.ASCII);
            BinaryWriter binWrtr = new BinaryWriter(File.Open(pathOut, FileMode.Create), Encoding.ASCII);
            //pathOut = pathOut.Replace(".bin", ".txt");
            //StreamWriter strmWrtr = new StreamWriter(pathOut);

            int length = binRdr.ReadInt32(); 
            byte miss = binRdr.ReadByte();
            byte beginWith = binRdr.ReadByte();
            double min = binRdr.ReadDouble(); 
            double max = binRdr.ReadDouble();  
            binWrtr.Write(length);
            binWrtr.Write(miss);
            binWrtr.Write(beginWith);
            binWrtr.Write(min);
            binWrtr.Write(max);
            //strmWrtr.Write(Convert.ToString(length) + "\r\n");
            //strmWrtr.Write(Convert.ToString(miss) + "\r\n");
            //strmWrtr.Write(Convert.ToString(beginWith) + "\r\n");
            //strmWrtr.Write(Convert.ToString(min) + "\r\n");
            //strmWrtr.Write(Convert.ToString(max) + "\r\n");

            byte serviceByte = (byte)(Math.Pow(2, bits) - 1);
            byte minDel = Byte.MinValue;
            byte maxDel = (byte)(serviceByte - 1);
            short maxNumbers = (short)(maxDel - minDel);

            List<short> sequence = new List<short>();
            short last = binRdr.ReadInt16();
            binWrtr.Write(last);
            //strmWrtr.Write(Convert.ToString(last) + "\r\n");
            int turns = 0;
            int totalOffset = last; //overall offset
            short offset = 0; //current offset
            bool frst = true;
            bool isEndOfFile = true;

            do
            {
                sequence.Clear(); //sequence of points
                short first;
                if (frst) //if it's the first passing
                    first = binRdr.ReadInt16();
                else
                    first = last;
                sequence.Add(first);

                //calculating offsets
                short offsetMin = Convert.ToInt16(first - maxNumbers - totalOffset);
                short offsetMax = Convert.ToInt16(first - totalOffset);

                isEndOfFile = true;
                
                while (binRdr.PeekChar() > -1)
                {
                    short cur = binRdr.ReadInt16(); //current point
                    sequence.Add(cur);
                    //above all axes
                    if (cur >= totalOffset + offsetMax * sequence.Count() &&
                        cur <= totalOffset + offsetMax * sequence.Count() + maxNumbers)
                    {
                        double tmp = Convert.ToDouble(cur - maxNumbers - totalOffset) / sequence.Count();
                        short newMin = Convert.ToInt16(Math.Ceiling(tmp));
                        if (newMin > offsetMin)
                            offsetMin = newMin;
                    }
                    //between axes
                    if (cur <= totalOffset + offsetMax * sequence.Count() &&
                        cur >= totalOffset + offsetMin * sequence.Count())
                    {
                        double tmp = Convert.ToDouble(cur - totalOffset) / sequence.Count();
                        short newMax = Convert.ToInt16(Math.Floor(tmp));
                        if (newMax < offsetMax)
                            offsetMax = newMax;
                        tmp = Convert.ToDouble(cur - maxNumbers - totalOffset) / sequence.Count();
                        short newMin = Convert.ToInt16(Math.Ceiling(tmp));
                        if (newMin > offsetMin)
                            offsetMin = newMin;
                    }
                    //out of range  
                    if (cur > totalOffset + offsetMax * sequence.Count() + maxNumbers ||
                        cur < totalOffset + offsetMin * sequence.Count() || offsetMax < offsetMin)
                    {
                        last = cur;
                        frst = false;
                        isEndOfFile = false;
                        break;
                    }
                }
                offset = offsetMax; //choosing an efficien offset
                binWrtr.Write(serviceByte);
                binWrtr.Write(offset);
                turns++;
                //strmWrtr.Write(Convert.ToString(serviceByte) + " " + Convert.ToString(offset));
                
                List<byte> plain = new List<byte>();
                int n;
                if (isEndOfFile)
                    n = sequence.Count();
                else
                    n = sequence.Count() - 1;
                for (int i = 0; i < n; i++)
                {
                    totalOffset += offset;
                    plain.Add(Convert.ToByte(sequence[i] - totalOffset));
                }

                binWrtr.Write(Convert.ToByte(plain.Count % 2));
                //strmWrtr.Write(" " + Convert.ToString(plain.Count % 2) + "\r\n");

                //bits shift
                List<byte> cipher = new List<byte>();
                byte package = 0;
                for (int i = 0; i < plain.Count(); i++)
                {
                    if (i % 2 == 0)
                    {
                        package = Convert.ToByte(plain[i] << 4);
                        if (i == plain.Count() - 1)
                            cipher.Add(package);
                    }
                    if (i % 2 == 1)
                    {
                        package = Convert.ToByte(package | plain[i]);
                        cipher.Add(package);
                    }
                }

                for (int i = 0; i < cipher.Count(); i++)
                {
                    binWrtr.Write(cipher[i]);
                    //strmWrtr.Write(Convert.ToString(cipher[i]) + "\r\n");
                }
            } while (binRdr.PeekChar() > -1);
            //strmWrtr.Write("turns: " + Convert.ToString(turns) + "\r\n");

            binRdr.Close();
            binWrtr.Close();
            //strmWrtr.Close();
        }

        static public void DecDelSmartOffset(string pathIn, string pathOut, byte bits = 4)
        {
            BinaryReader binRdr = new BinaryReader(File.Open(pathIn, FileMode.Open), Encoding.ASCII);
            BinaryWriter binWrtr = new BinaryWriter(File.Open(pathOut, FileMode.Create), Encoding.ASCII);
            //pathOut = pathOut.Replace(".bin", ".txt");
            //StreamWriter strmWrtr = new StreamWriter(pathOut);

            int length = binRdr.ReadInt32();
            byte miss = binRdr.ReadByte();
            byte beginWith = binRdr.ReadByte();
            double min = binRdr.ReadDouble();
            double max = binRdr.ReadDouble();
            binWrtr.Write(length);
            binWrtr.Write(miss);
            binWrtr.Write(beginWith);
            binWrtr.Write(min);
            binWrtr.Write(max);
            //strmWrtr.Write(Convert.ToString(length) + "\r\n");
            //strmWrtr.Write(Convert.ToString(miss) + "\r\n");
            //strmWrtr.Write(Convert.ToString(beginWith) + "\r\n");
            //strmWrtr.Write(Convert.ToString(min) + "\r\n");
            //strmWrtr.Write(Convert.ToString(max) + "\r\n");

            byte serviceByte = (byte)(Math.Pow(2, bits) - 1);
            short offset = 0;
            byte isEven = 0;
            int totalOffset = binRdr.ReadInt16();
            binWrtr.Write((short)totalOffset);
            //strmWrtr.Write(Convert.ToString(totalOffset) + "\r\n");

            List<byte> sequence = new List<byte>();
            binRdr.ReadByte();
            do
            {
                offset = binRdr.ReadInt16();
                isEven = binRdr.ReadByte();
                while(binRdr.PeekChar() > -1)
                {
                    byte cur = binRdr.ReadByte();
                    if (cur == serviceByte)
                        break;
                    sequence.Add(Convert.ToByte(cur >> 4));
                    sequence.Add(Convert.ToByte(cur & 15));
                }
                int n;
                if (isEven == 0)
                    n = sequence.Count();
                else
                    n = sequence.Count() - 1;
                for (int i = 0; i < n; i++)
                {
                    totalOffset += offset;
                    short res = Convert.ToInt16(totalOffset + sequence[i]);
                    binWrtr.Write(res);
                    //strmWrtr.Write(Convert.ToString(res + "\r\n"));
                }
                sequence.Clear();
            } while(binRdr.PeekChar() > -1);

            binRdr.Close();
            binWrtr.Close();
            //strmWrtr.Close();
        }
        
        static public void DecDelShort(string pathIn, string pathOut)
        {
            BinaryReader binRdr = new BinaryReader(File.Open(pathIn, FileMode.Open), Encoding.ASCII);
            BinaryWriter binWrtr = new BinaryWriter(File.Open(pathOut, FileMode.Create), Encoding.ASCII);
            //pathOut = pathOut.Replace(".bin", ".txt");
            //StreamWriter strmWrtr = new StreamWriter(pathOut);

            int length = binRdr.ReadInt32();
            byte miss = binRdr.ReadByte();
            byte beginWith = binRdr.ReadByte();
            double min = binRdr.ReadDouble();
            double max = binRdr.ReadDouble();
            binWrtr.Write(length);
            binWrtr.Write(miss);
            binWrtr.Write(beginWith);
            binWrtr.Write(min);
            binWrtr.Write(max);
            //strmWrtr.Write(Convert.ToString(length) + "\r\n");
            //strmWrtr.Write(Convert.ToString(miss) + "\r\n");
            //strmWrtr.Write(Convert.ToString(beginWith) + "\r\n");
            //strmWrtr.Write(Convert.ToString(min) + "\r\n");
            //strmWrtr.Write(Convert.ToString(max) + "\r\n");

            short last = binRdr.ReadInt16();
            binWrtr.Write(last);
            //strmWrtr.Write(Convert.ToString(last) + "\r\n");
            do
            {
                short cur = binRdr.ReadInt16();
                short res = (short)(last + cur);
                binWrtr.Write(res);
                //strmWrtr.Write(Convert.ToString(res) + "\r\n");
                last = res;
            } while (binRdr.PeekChar() > -1);
            binRdr.Close();
            binWrtr.Close();
            //strmWrtr.Close();
        }

        static public void Sampling(string pathIn, string pathOut, byte miss, byte beginWith = 0)
        {
            BinaryReader binRdr = new BinaryReader(File.Open(pathIn, FileMode.Open), Encoding.ASCII);
            BinaryWriter binWrtr = new BinaryWriter(File.Open(pathOut, FileMode.Create), Encoding.ASCII);
            //pathOut = pathOut.Replace(".bin", ".txt");
            //StreamWriter strmWrtr = new StreamWriter(pathOut);

            int length = 0;
            do
            {
                binRdr.ReadDouble();
                length++;
            } while (binRdr.PeekChar() > -1);
            binWrtr.Write(length);
            binWrtr.Write(miss);
            binWrtr.Write(beginWith);
            //strmWrtr.Write(Convert.ToString(length) + "\r\n");
            //strmWrtr.Write(Convert.ToString(miss) + "\r\n");
            //strmWrtr.Write(Convert.ToString(beginWith) + "\r\n");
            binRdr.BaseStream.Position = 0;

            for (int i = 0; i < beginWith; i++)
                binRdr.ReadDouble();
            
            do
            {
                double number = binRdr.ReadDouble();
                binWrtr.Write(number);
                //strmWrtr.Write(Convert.ToString(number) + "\r\n");
                for (int i = 0; i < miss; i++)
                {
                    if (binRdr.PeekChar() > -1)
                        number = binRdr.ReadDouble();
                    else
                        break;
                }
            } while (binRdr.PeekChar() > -1);

            binRdr.Close();
            binWrtr.Close();
            //strmWrtr.Close();
        }

        static public void Interpolation(string pathIn, string pathOut)
        {
            BinaryReader binRdr = new BinaryReader(File.Open(pathIn, FileMode.Open), Encoding.ASCII);
            BinaryWriter binWrtr = new BinaryWriter(File.Open(pathOut, FileMode.Create), Encoding.ASCII);
            pathOut = pathOut.Replace(".bin", ".txt");
            StreamWriter strmWrtr = new StreamWriter(pathOut);

            int originLength = binRdr.ReadInt32();
            int restoredLength = 0;
            byte missed = binRdr.ReadByte();
            byte beginWith = binRdr.ReadByte();

            int firstX = beginWith, middleX = 0, lastX = 0;

            double firstY = 0, middleY = 0, lastY = 0;
            firstY = binRdr.ReadDouble();
            bool firstPass = true;
            double a1, a2, a0;

            do
            {
                int beginning; 
                double testMiddle = binRdr.ReadDouble();
                if (binRdr.PeekChar() > -1)
                {
                    middleY = testMiddle;
                    middleX = firstX + missed + 1;
                    lastY = binRdr.ReadDouble();
                    lastX = middleX + missed + 1;
                    beginning = firstX + 1;
                }
                else
                {
                    firstX = middleX;
                    firstY = middleY;
                    middleX = lastX;
                    middleY = lastY;
                    lastY = testMiddle;
                    lastX = middleX + missed + 1;
                    beginning = middleX + 1;
                }
                a2 = (lastY - firstY) / ((lastX - firstX) * (lastX - middleX)) -
                    (middleY - firstY) / ((middleX - firstX) * (lastX - middleX));
                a1 = (middleY - firstY) / (middleX - firstX) - a2 * (middleX + firstX);
                a0 = firstY - a1 * firstX - a2 * firstX * firstX;
                if (firstPass)
                {
                    for (int i = 0; i <= lastX; i++)
                    {
                        double x = a0 + a1 * i + a2 * i * i;
                        binWrtr.Write(x);
                        restoredLength++;
                        strmWrtr.Write(Convert.ToString(x) + "\r\n");
                    }
                    firstPass = false;
                }
                else
                {
                    for (int i = beginning; i <= lastX; i++)
                    {
                        double x = a0 + a1 * i + a2 * i * i;
                        binWrtr.Write(x);
                        restoredLength++;
                        strmWrtr.Write(Convert.ToString(x) + "\r\n");
                    }
                }
                firstX = lastX;
                firstY = lastY;
            } while(binRdr.PeekChar() > -1);

            for (int i = restoredLength; i < originLength; i++)
            {
                double x = a0 + a1 * i + a2 * i * i;
                binWrtr.Write(x);
                restoredLength++;
                strmWrtr.Write(Convert.ToString(x) + "\r\n");
            }

            binRdr.Close();
            binWrtr.Close();
            strmWrtr.Close();
        }

        static public double CalcDiff(string pathOrigin, string pathRestored)
        {
            BinaryReader binOrigin = new BinaryReader(File.Open(pathOrigin, FileMode.Open), Encoding.ASCII);
            BinaryReader binRestored = new BinaryReader(File.Open(pathRestored, FileMode.Open), Encoding.ASCII);

            double result = 0;
            do
            {
                double curRestored = binRestored.ReadDouble();
                double curOrigin = binOrigin.ReadDouble();
                result += Math.Abs(curRestored - curOrigin);
            } while (binRestored.PeekChar() > -1);

            binOrigin.Close();
            binRestored.Close();

            return result;
        }

        static public double CalcPercentDiff(string pathOrigin, string pathRestored)
        {
            BinaryReader binOrigin = new BinaryReader(File.Open(pathOrigin, FileMode.Open), Encoding.ASCII);
            BinaryReader binRestored = new BinaryReader(File.Open(pathRestored, FileMode.Open), Encoding.ASCII);

            double max, min;
            max = min = binOrigin.ReadDouble();
            do
            {
                double cur = binOrigin.ReadDouble();
                if (cur < min)
                    min = cur;
                if (cur > max)
                    max = cur;
            } while (binOrigin.PeekChar() > -1);

            binOrigin.BaseStream.Position = 0;
            double diff = max - min;
            double maxPercent = 0;
            do
            {
                double curRestored = binRestored.ReadDouble();
                double curOrigin = binOrigin.ReadDouble();
                double percent = Math.Abs(curRestored - curOrigin) / diff;
                if (percent > maxPercent)
                    maxPercent = percent;
            } while (binRestored.PeekChar() > -1);

            binOrigin.Close();
            binRestored.Close();

            return maxPercent;
        }
    }
}
