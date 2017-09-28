using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// Чтение и запись Wave-файла
// Поддерживает только 16-битный формат
// Данные чтения хранятся в списке<short>


namespace FourierTransform
{
    class WavReader
    {
        struct WavHeader
        {
            public byte[] riffID; // "riff"
            public uint size;  // ファイルサイズ-8
            public byte[] wavID;  // "WAVE"
            public byte[] fmtID;  // "fmt "
            public uint fmtSize; // fmtチャンクのバイト数
            public ushort format; // フォーマット
            public ushort channels; // チャンネル数
            public uint sampleRate; // サンプリングレート
            public uint bytePerSec; // データ速度
            public ushort blockSize; // ブロックサイズ
            public ushort bit;  // 量子化ビット数
            public byte[] dataID; // "data"
            public uint dataSize; // 波形データのバイト数
        }
        override public String ToString()
        {
            return "riffID: " + Header.riffID +
                "\nsize: " + Header.size +
                "\nchannels: " + Header.channels +
                "\nsample rate: " + Header.sampleRate + 
                "\nbyte per second: " + Header.bytePerSec + 
                "\ndata size: " + Header.dataSize;
        }
        private  WavHeader Header = new WavHeader();
        public List<short> lDataList = new List<short>();
        public List<short> rDataList = new List<short>();

        public uint getSampleRate()
        {
            return Header.sampleRate;
        }

        public void read(String file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                try
                {
                    Header.riffID = br.ReadBytes(4);
                    Header.size = br.ReadUInt32();
                    Header.wavID = br.ReadBytes(4);
                    Header.fmtID = br.ReadBytes(4);
                    Header.fmtSize = br.ReadUInt32();
                    Header.format = br.ReadUInt16();
                    Header.channels = br.ReadUInt16();
                    Header.sampleRate = br.ReadUInt32();
                    Header.bytePerSec = br.ReadUInt32();
                    Header.blockSize = br.ReadUInt16();
                    Header.bit = br.ReadUInt16();
                    Header.dataID = br.ReadBytes(4);
                    Header.dataSize = br.ReadUInt32();

                    for (int i = 0; i < Header.dataSize / Header.blockSize; i++)
                    {
                        lDataList.Add((short)br.ReadUInt16());
                        rDataList.Add((short)br.ReadUInt16());
                    }
                }
                finally
                {
                    if (br != null)
                    {
                        br.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
        }
        public void write(String file)
        {

            Header.dataSize = (uint)Math.Max(lDataList.Count, rDataList.Count) * 4;

            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                try
                {
                    bw.Write(Header.riffID);
                    bw.Write(Header.size);
                    bw.Write(Header.wavID);
                    bw.Write(Header.fmtID);
                    bw.Write(Header.fmtSize);
                    bw.Write(Header.format);
                    bw.Write(Header.channels);
                    bw.Write(Header.sampleRate);
                    bw.Write(Header.bytePerSec);
                    bw.Write(Header.blockSize);
                    bw.Write(Header.bit);
                    bw.Write(Header.dataID);
                    bw.Write(Header.dataSize);

                    for (int i = 0; i < Header.dataSize / Header.blockSize; i++)
                    {
                        if (i < lDataList.Count)
                        {
                            bw.Write((ushort)lDataList[i]);
                        }
                        else
                        {
                            bw.Write(0);
                        }

                        if (i < rDataList.Count)
                        {
                            bw.Write((ushort)rDataList[i]);
                        }
                        else
                        {
                            bw.Write(0);
                        }
                    }
                }
                finally
                {
                    if (bw != null)
                    {
                        bw.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }

            return;
        }
    }        
}
