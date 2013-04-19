#ifndef TAIVAANVAHTITEST_H
#define TAIVAANVAHTITEST_H

#include <QObject>
#include "taivaanvahti.h"
#include "taivaanvahtifield.h"

class TaivaanvahtiTest : public QObject
{
    Q_OBJECT
public:
    explicit TaivaanvahtiTest(QObject *parent = 0);
    void runTest();
signals:
    
public slots:
private slots:
    void formReceived(QVector<TaivaanvahtiField*> &fields);
private:
    Taivaanvahti tv;
};

#endif // TAIVAANVAHTITEST_H
