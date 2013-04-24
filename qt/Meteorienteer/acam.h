#ifndef ACAM_H
#define ACAM_H

#include <QDeclarativeItem>

class ACam : public QDeclarativeItem
{
    Q_OBJECT
public:
    explicit ACam(QDeclarativeItem *parent = 0);

protected:
    void paint(QPainter *,
                     const QStyleOptionGraphicsItem *,
                     QWidget * );
    
signals:
    
public slots:
    
};

#endif // ACAM_H
