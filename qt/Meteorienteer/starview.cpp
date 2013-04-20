#include "starview.h"

StarView::StarView(QDeclarativeItem *parent) :
    QDeclarativeItem(parent)
{
    setFlag(QGraphicsItem::ItemHasNoContents, false);
    setFlag(QGraphicsItem::ItemDoesntPropagateOpacityToChildren, true);

    //setAttribute(Qt::WA_TranslucentBackground);
    //setStyleSheet("background:transparent;");
}

void StarView::paint(QPainter *painter,
                           const QStyleOptionGraphicsItem *option,
                           QWidget *widget)
 {
    Q_UNUSED(option)
    Q_UNUSED(widget)
//    Q_UNUSED(painter)
    QPainter pain(widget);
    pain.fillRect(widget->rect(), Qt::red);
 }
