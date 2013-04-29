#ifndef ACAM_H
#define ACAM_H

#include <QDeclarativeItem>
#include <QCamera>
#include <qsystemscreensaver.h>
#include <avideosurface.h>

class ACam : public QDeclarativeItem
{
    Q_OBJECT
public:
    explicit ACam(QDeclarativeItem *parent = 0);

protected:
    void paint(QPainter *,
                     const QStyleOptionGraphicsItem *,
                     QWidget * );
    AVideoSurface*         m_videoSurface;
    //QSystemScreenSaver *     m_systemScreenSaver;
signals:
    
public slots:
    
};

#endif // ACAM_H
