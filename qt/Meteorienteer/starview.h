#ifndef STARVIEW_H
#define STARVIEW_H

#include <QDeclarativeItem>
#include <QGraphicsItem>
#include <QPainter>

class StarView : public QDeclarativeItem
{
    Q_OBJECT
public:
    explicit StarView(QDeclarativeItem *parent = 0);
    void paint(QPainter *painter,
                               const QStyleOptionGraphicsItem *option,
                               QWidget *widget);
signals:
    
public slots:
    
};

#endif // STARVIEW_H
