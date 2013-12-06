/// <reference path="typings\knockout\knockout.d.ts" />
/// <reference path="typings\knockout.mapping\knockout.mapping.d.ts" />
/// <amd-dependency path="knockout"/>
var ssw;
(function (ssw) {
    var knockout;
    (function (knockout) {
        var DisableExHandler = (function () {
            function DisableExHandler(handler) {
                var _this = this;
                this.defaultHandler = handler;

                this.init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    if (_this.defaultHandler.init) {
                        _this.defaultHandler.init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
                    }

                    $(element).click(function (evt) {
                        if (valueAccessor()) {
                            evt.preventDefault();
                        }
                    });
                };

                this.update = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    if (_this.defaultHandler.init) {
                        _this.defaultHandler.init(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext);
                    }
                };
            }
            return DisableExHandler;
        })();
        var SelectedValueHandler = (function () {
            function SelectedValueHandler() {
            }
            SelectedValueHandler.prototype.init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var value = valueAccessor();
                var $element = $(element);
                if (ko.isObservable(value)) {
                    $element.change(function (e) {
                        var v = $element.val();
                        value(v);
                        console.log(v);
                    });
                }
            };
            SelectedValueHandler.prototype.update = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var val = ko.utils.unwrapObservable(valueAccessor());
                $(element).val(val);
            };
            return SelectedValueHandler;
        })();
        var ToggleHandler = (function () {
            function ToggleHandler() {
            }
            ToggleHandler.prototype.init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                // attach to click event
                var $element = jQuery(element);
                var editMode = allBindingsAccessor().EditMode;
                $element.on("click", function () {
                    var enabled = true;
                    if (typeof editMode !== 'undefined') {
                        enabled = ko.utils.unwrapObservable(editMode);
                    }
                    if (enabled) {
                        var obs = valueAccessor();
                        var new_val = !obs();
                        obs(new_val);
                    }
                });
            };
            ToggleHandler.prototype.update = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                var observable = valueAccessor();
                var val = observable();
                var $element = jQuery(element);
                if (val) {
                    $element.addClass("active");
                } else {
                    $element.removeClass("active");
                }
            };
            return ToggleHandler;
        })();
        ;

        var handlers = ko.bindingHandlers;
        handlers.selectedValue = new SelectedValueHandler();
        handlers.disable = new DisableExHandler(ko.bindingHandlers.disable);
        handlers.toggle = new ToggleHandler();
    })(knockout || (knockout = {}));
})(ssw || (ssw = {}));
//# sourceMappingURL=ssw.knockout.js.map
