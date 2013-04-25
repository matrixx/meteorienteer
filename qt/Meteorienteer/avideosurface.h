#ifndef MYVIDEOSURFACE_H
#define MYVIDEOSURFACE_H

#include <QAbstractVideoSurface>
#include <QVideoRendererControl>
#include <QVideoSurfaceFormat>

#include "uyvy2rgb.h"

/*****************************************************************************
* MyVideoSurface
*/
class VideoIF
{
public:
    virtual void updateVideo() = 0;
};

class MyVideoSurface: public QAbstractVideoSurface
{
Q_OBJECT

public:
    MyVideoSurface(QWidget* widget, VideoIF* target, QObject * parent = 0);
    ~MyVideoSurface();

    bool start(const QVideoSurfaceFormat &format);

    bool present(const QVideoFrame &frame);

    QList<QVideoFrame::PixelFormat> supportedPixelFormats(
                QAbstractVideoBuffer::HandleType handleType = QAbstractVideoBuffer::NoHandle) const;

    void paint(QPainter*);

    void calcLuma(uchar* luma, uchar* rgb, int width);
    void edgify(QImage* image);

private:
    QWidget* m_targetWidget;
    VideoIF* m_target;
    QVideoFrame m_frame;
    QImage::Format m_imageFormat;
    int lumaR[256], lumaG[256], lumaB[256];
    int sqrt_table[256][256];

    UYVY2RGB uyvy2rgb;
};

#endif // MYVIDEOSURFACE_H
