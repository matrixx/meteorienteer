#include "formmanager.h"

FormManager::FormManager(QObject *parent) :
    QObject(parent)
{
}

void FormManager::receiveForm(TaivaanvahtiForm* form)
{
    m_form = form;
    form->setParent(this);
}
