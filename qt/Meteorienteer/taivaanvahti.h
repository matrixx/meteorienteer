#ifndef TAIVAANVAHTI_H
#define TAIVAANVAHTI_H

#include <QObject>
#include <QNetworkAccessManager>
#include <QDomElement>
#include <QVector>
#include <QMap>
#include <QVariantList>

class TaivaanvahtiField;
class TaivaanvahtiForm;
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
    Q_INVOKABLE void getForm(int category);
    void submitForm(FormData form, int category);
signals:
    void formReceived(TaivaanvahtiForm* form);
    void formSubmitted(bool success, int observationId, QString key);

private slots:
    void getFormFinished();
    void submitFormFinished();
private:
    QString readFile(QString filename);
    void handleCategory(QDomElement categoryElem, QVector<TaivaanvahtiField*> &fields, TaivaanvahtiForm* form);
    QNetworkAccessManager nam;
};

#endif // TAIVAANVAHTI_H
