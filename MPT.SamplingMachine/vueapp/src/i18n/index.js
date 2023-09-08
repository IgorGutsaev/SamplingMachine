import { createI18n } from 'vue-i18n'

const i18n = createI18n({
    locale: 'hi',   // default locale
    // translations
    messages: {
        hi: {
            titles: {
                terms: 'शर्तें',
                termsConfirmation: 'मैं सेवा की शर्तों और गोपनीयता नीति को स्वीकार करता हूं',
                phoneNumber: 'आपका फोन नंबर',
                pinCode: 'पिन कोड',
                credit: 'श्रेय',
                lackOfCredit: 'अपर्याप्त क्रेडिट',
                outOfStock: 'स्टॉक ख़त्म',
                itemsCount: 'आइटम गिनती',
                takeYourProducts: 'अपने उत्पाद ले लो',
                goodBuy: 'अच्छी खरीद!',
                formatNumber: 'प्रारूप: xxxxxxxxxx',
                formatCode: 'प्रारूप: xxxx'
            },
            buttons: {
                next: 'अगला',
                add: 'जोड़ना',
                remove: 'निकालना',
                more: 'अधिक',
                dispense: 'उत्पाद जारी करें',
                sendSMS: 'एसएमएस भेजें',
                confirm: 'पुष्टि करना'
            }
        },
        en: {
            titles: {
                terms: 'Terms',
                termsConfirmation: 'I accept the Terms of service and Privacy Policy',
                phoneNumber: 'Your phone number',
                pinCode: 'PIN code',
                credit: 'Credit',
                lackOfCredit: 'Insufficient credit',
                outOfStock: 'Out of stock',
                itemsCount: 'Items count',
                takeYourProducts: 'Take your products',
                goodBuy: 'Good buy!',
                formatNumber: 'format: xxxxxxxxxx',
                formatCode: 'format: xxxx'
            },
            buttons: {
                next: 'Next',
                add: 'Add',
                remove: 'Remove',
                more: 'More',
                dispense: 'Issue products',
                sendSMS: 'Send SMS',
                confirm: 'Confirm'
            }
        }
    }
})

export default i18n