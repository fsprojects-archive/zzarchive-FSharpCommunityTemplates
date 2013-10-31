(function ( mod, $ ) {
    mod.buildTemplateUrl = function ( templateFileName ) {
        return "/Templates/" + templateFileName;
    };

    mod.serializeObject = function ( selector ) {
        var result = {};
        var serializedArray = $( selector ).serializeArray( { checkboxesAsBools: true } );
        $.each(serializedArray, function () {
            if ( result[this.name] ) {
                if ( !result[this.name].push ) {
                    result[this.name] = [result[this.name]];
                }
                result[this.name].push( this.value || "" );
            } else {
                result[this.name] = this.value || "";
            }
        });
        return result;
    };
})( window.appFsMvc.utility = window.appFsMvc.utility || {}, jQuery );