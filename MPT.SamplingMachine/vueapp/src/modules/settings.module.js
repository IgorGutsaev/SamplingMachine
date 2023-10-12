export default {
    props: {
        isOn: {
            type: Boolean,
            required: true
        },
        currentLanguage: {
            type: Object,
            required: false
        },
        languages: {
            type: Object,
            required: false
        },
        // reflects max samples count per session
        credit: {
            type: Number,
            required: true
        },
        idleTimeoutSec: {
            type: Number,
            required: true
        },
        canLogOff: {
            type: Boolean,
            required: true
        },
        media: {
            type: Object,
            required: false
        },
    }
}