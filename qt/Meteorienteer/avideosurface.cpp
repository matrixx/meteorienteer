//#include "cameraexample.h"
#include "qmath.h"

/*****************************************************************************
* AVideoSurface
*/
AVideoSurface::AVideoSurface(QWidget* widget, VideoIF* target, QObject* parent)
    : QAbstractVideoSurface(parent)
{
    m_targetWidget = widget;
    m_target = target;
    m_imageFormat = QImage::Format_Invalid;

    // Y = 0.2126 R + 0.7152 G + 0.0722 B.
    // Y = (54 R + 183 G + 18 B) >> 8
    int i = 0;
    for (i = 0; i < 256; i++)
        lumaR[i] = 54*i;
    for (i = 0; i < 256; i++)
        lumaG[i] = 183*i;
    for (i = 0; i < 256; i++)
        lumaB[i] = 18*i;

    for (int y = 0; y < 255; y++)
        for (int x = 0; x < y; x++)
            {
            int val = (int)qSqrt(x*x+y*y);
            val = val > 255 ? 255 : val;
            sqrt_table[x][y] = val;
            sqrt_table[y][x] = val;
            }
}

AVideoSurface::~AVideoSurface()
{
}

bool AVideoSurface::start(const QVideoSurfaceFormat &format)
{
    QImage::Format imageFormat = QVideoFrame::imageFormatFromPixelFormat(format.pixelFormat());

    if (format.pixelFormat() == QVideoFrame::Format_UYVY)
        imageFormat = QImage::Format_RGB32;

    const QSize size = format.frameSize();

    if (imageFormat != QImage::Format_Invalid && !size.isEmpty()) {
        m_imageFormat = imageFormat;
        QAbstractVideoSurface::start(format);
        return true;
    } else {
        return false;
    }
}

bool AVideoSurface::present(const QVideoFrame &frame)
{
    m_frame = frame;
    if (surfaceFormat().pixelFormat() != m_frame.pixelFormat() ||
            surfaceFormat().frameSize() != m_frame.size()) {
        stop();
        return false;
    } else {
        m_target->updateVideo();
        return true;
    }
}

void AVideoSurface::calcLuma(uchar* luma, uchar* rgb, int width)
{
    int i = 0;
    uchar* pos = rgb;
    for (i = 0; i < width; i++)
    {
        uchar lv = (lumaB[*pos] + lumaG[*(pos+1)] + lumaR[*(pos+2)]) >> 8;
        *luma = lv;
        pos+=4;
        luma++;
    }
}

void AVideoSurface::edgify(QImage* image)
{
    if (image->format() != QImage::Format_RGB32 && image->format() != QVideoFrame::Format_ARGB32)
        return;

    uchar* start = image->bits();
    int bpl = image->bytesPerLine();

    uchar* l1 = new uchar[image->width()];
    uchar* l2 = new uchar[image->width()];
    uchar* l3 = new uchar[image->width()];
    uchar* lt;

    uchar* nxt_line = start;
    calcLuma(l2, nxt_line, image->width());
    nxt_line += bpl;
    calcLuma(l3, nxt_line, image->width());

    uchar* mod_line = start + bpl;

    for (int y = 1; y < image->height()-1; y++)
    {
        // swap lines and copy next line to 3rd item
        lt = l1;
        l1 = l2;
        l2 = l3;
        l3 = lt;
        nxt_line += bpl;
        calcLuma(l3, nxt_line, image->width());

        // calc gradients

        int spos = 1;
        int dpos = 4;
        for (int x = 1; x < image->width()-1; x++)
        {
            int dx = qAbs(- l1[spos-1] - l2[spos-1]*2 - l3[spos-1] + l1[spos+1] + l2[spos+1]*2 + l3[spos+1]);
            int dy = qAbs(- l1[spos-1] - l1[spos  ]*2 - l1[spos+1] + l3[spos-1] + l3[spos  ]*2 + l3[spos+1]);
            int v;

            // Try using this instead of sqrt_table to see performance penalty
            //v = qSqrt(dx*dx+dy*dy);
            //if (v > 255)
            //    v = 255;

            if (dx > 255 || dy > 255)
                v = 255;
            else
                v = sqrt_table[dx][dy];

            *(mod_line+dpos  ) = v;
            *(mod_line+dpos+1) = v;
            *(mod_line+dpos+2) = v;

            spos += 1;
            dpos += 4;
        }

        mod_line += bpl;
    }

    delete[] l1;
    delete[] l2;
    delete[] l3;
}


void AVideoSurface::paint(QPainter *painter)
    {
    if (m_frame.map(QAbstractVideoBuffer::ReadOnly))
        {
        QImage* image;

        if (m_frame.pixelFormat() == QVideoFrame::Format_UYVY)
            {
            uyvy2rgb.initData(m_frame);

            image = new QImage(uyvy2rgb.bits(),
                               uyvy2rgb.width(),
                               uyvy2rgb.height(),
                               uyvy2rgb.width() * 4,
                               m_imageFormat);
            }
        else
            {
            image = new QImage(m_frame.bits(),
                               m_frame.width(),
                               m_frame.height(),
                               m_frame.bytesPerLine(),
                               m_imageFormat);
            edgify(image);
            }


        QRect r = m_targetWidget->rect();
        QPoint centerPic((qAbs(r.size().width() - image->size().width())) / 2,
                         (qAbs(r.size().height() - image->size().height())) / 2);

        if (!image->isNull())
            {
            painter->drawImage(centerPic, *image);
            }

        delete image;

        m_frame.unmap();
        }
    }

QList<QVideoFrame::PixelFormat> AVideoSurface::supportedPixelFormats(
            QAbstractVideoBuffer::HandleType handleType) const
{
    if (handleType == QAbstractVideoBuffer::NoHandle) {
        return QList<QVideoFrame::PixelFormat>()
                << QVideoFrame::Format_RGB32
                << QVideoFrame::Format_ARGB32
                << QVideoFrame::Format_ARGB32_Premultiplied
                << QVideoFrame::Format_RGB565
                << QVideoFrame::Format_RGB555
                << QVideoFrame::Format_UYVY;
    } else {
        return QList<QVideoFrame::PixelFormat>();
    }
}
