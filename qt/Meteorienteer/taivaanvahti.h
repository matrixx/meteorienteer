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
    // Form submission data. Field, value
    typedef QMap<TaivaanvahtiField*, QString> FormData;

    explicit Taivaanvahti(QObject *parent = 0);
    // Downloads the form and emits formReceived
    Q_INVOKABLE void getForm(int category);
    // Submits the form data and emits formSubmitted
    void submitForm(FormData form, int category);
signals:
    // Note: ownership of form changes! User must delete it.
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
