#ifndef TAIVAANVAHTI_H
#define TAIVAANVAHTI_H

#include <QObject>
#include <QNetworkAccessManager>
#include <QDomElement>
#include <QVector>
#include <QMap>

class TaivaanvahtiField;

/**
 * The Taivaanvahti class is a client class for
 * Taivaanvahti observation reporting system.
 *
 * Usage:
 * Connect formReceived() to your handler.
 * call getForm() to get the form. Category 1 would be fireballs.
 * formReceived() is emitted. In error fields is empty.
 */
class Taivaanvahti : public QObject
{
    Q_OBJECT
public:
    typedef QMap<TaivaanvahtiField*, QString> FormData;

    explicit Taivaanvahti(QObject *parent = 0);
    void getForm(int category);
    void submitForm(FormData form, int category);
signals:
    // fields is valid only during call and will be deleted after.
    void formReceived(QVector<TaivaanvahtiField*> &fields);

private slots:
    void getFormFinished();
    void submitFormFinished();
private:
    QString readFile(QString filename);
    void handleCategory(QDomElement categoryElem, QVector<TaivaanvahtiField*> &fields);
    QNetworkAccessManager nam;
};

#endif // TAIVAANVAHTI_H
